using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TamsApi.Core.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TamsApi.Models;
using TamsApi.Models.Lookups;

namespace TamsApi.Data
{
    public partial class ApplicationContext
    {
        private readonly ISystemClock _clock;
        private static DateTime NullDate = default(DateTime);

        public ApplicationContext(DbContextOptions<ApplicationContext> options, ISystemClock clock)
            : base(options)
        {
            _clock = clock;
        }

        #region Entities added after scaffold
        public DbSet<ExcelFailure> ExcelFailures { get; set; }
        #endregion

        public virtual int SaveChanges(AuthenticatedUser user)
        {
            StampChangedEntries(user);
            return base.SaveChanges();
        }

        public virtual Task<int> SaveChangesAsync(AuthenticatedUser user)
        {
            StampChangedEntries(user);
            return base.SaveChangesAsync();
        }

        private void StampChangedEntries(AuthenticatedUser user = null)
        {
            var now = _clock.UtcNow.UtcDateTime;
            var userId = user?.UserId;
            var unstampedEntries = ChangeTracker.Entries<ITrackModified>().Where(e => e.IsModified());
            foreach(var entry in unstampedEntries)
            {
                var entity = entry.Entity;
                entity.LastModifiedDate = now;
                if (userId != null) entity.LastModifiedUserId = userId.Value;

                if (entity is ITrackCreated && entry.State == EntityState.Added)
                {
                    var trackedEntity = (ITrackCreated)entity;
                    if (trackedEntity.CreatedDate == NullDate) trackedEntity.CreatedDate = now;
                    if (trackedEntity.CreatedUserId == default && userId.HasValue) trackedEntity.CreatedUserId = userId.Value;
                }

                if (entity is ITrackRevisions)
                {
                    IncrementRevision((ITrackRevisions)entity, entry.State);
                }
            }

            var addEntries = ChangeTracker.Entries<ITrackCreated>().Where(e => e.State == EntityState.Added && (e.Entity.CreatedDate == NullDate || e.Entity.CreatedUserId == default));
            foreach (var entry in addEntries)
            {
                var entity = entry.Entity;
                if (entity.CreatedDate == NullDate) entity.CreatedDate = now;
                if (entity.CreatedUserId == default && userId.HasValue) entity.CreatedUserId = userId.Value;
            }
        }

        private void IncrementRevision(ITrackRevisions entity, EntityState state)
        {
            if (state == EntityState.Added || state == EntityState.Unchanged) return;

            entity.Revision = entity.Revision + 1;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<VInputStreamSummary> VInputStreamSummary { get; set; }
        public virtual DbSet<ActivityReport> GetActivityReport { get; set; }
        public virtual DbSet<VLogTypeStatus> VLogTypeStatus { get; set; }
        public virtual DbSet<VPaymentApprovalReport> VPaymentApprovalReport { get; set; }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogType>().HasData(
                new LogType { LogTypeId = (int) LogTypeId.Add, Name = "Add", IsActive = true },
                new LogType { LogTypeId = (int) LogTypeId.Cancellation, Name = "Cancellation", IsActive = true },
                new LogType { LogTypeId = (int) LogTypeId.Escalation, Name = "Escalation", IsActive = true },
                new LogType { LogTypeId = (int) LogTypeId.Modification, Name = "Modification", IsActive = true },
                new LogType { LogTypeId = (int) LogTypeId.ServiceRequest, Name = "Service Request", IsActive = true });

            modelBuilder.Entity<FileType>().HasData(
                new FileType { FileTypeId = 1, Name = "Change Log", IsActive = true },
                new FileType { FileTypeId = 2, Name = "Dispute Log", IsActive = true },
                new FileType { FileTypeId = 3, Name = "Invoice", IsActive = true },
                new FileType { FileTypeId = 4, Name = "Payment", IsActive = true },
                new FileType { FileTypeId = 5, Name = "TSA Schedule", IsActive = true },
                new FileType { FileTypeId = 6, Name = "Other", IsActive = true },
                new FileType { FileTypeId = 7, Name = "Add Log Attachment", IsActive = true },
                new FileType { FileTypeId = 8, Name = "Sell Side Notification Change", IsActive = true },
                new FileType { FileTypeId = 9, Name = "Sell Side Notification Dispute", IsActive = true },
                new FileType { FileTypeId = 10, Name = "Change Log Attachment", IsActive = true },
                new FileType { FileTypeId = 11, Name = "Dispute Log Attachment", IsActive = true });

            modelBuilder.Entity<LogStatus>().HasData(
                new LogStatus { LogStatusId = 1, Name = "Add Pending", LogTypeId = 1, IsActive = true },
                new LogStatus { LogStatusId = 2, Name = "Add Confirmed", LogTypeId = 1, IsActive = true },
                new LogStatus { LogStatusId = 3, Name = "Cancellation Pending", LogTypeId = 2, IsActive = true },
                new LogStatus { LogStatusId = 4, Name = "Cancellation Confirmed", LogTypeId = 2, IsActive = true },
                new LogStatus { LogStatusId = 5, Name = "Cancellation Rejected", LogTypeId = 2, IsActive = true },
                new LogStatus { LogStatusId = 6, Name = "Escalation Pending", LogTypeId = 3, IsActive = true },
                new LogStatus { LogStatusId = 7, Name = "Escalation Resolved", LogTypeId = 3, IsActive = true },
                new LogStatus { LogStatusId = 8, Name = "Modification Pending", LogTypeId = 4, IsActive = true },
                new LogStatus { LogStatusId = 9, Name = "Modification Confirmed", LogTypeId = 4, IsActive = true },
                new LogStatus { LogStatusId = 10, Name = "Modification Rejected", LogTypeId = 4, IsActive = true },
                new LogStatus { LogStatusId = 11, Name = "Service Request Pending", LogTypeId = 5, IsActive = true },
                new LogStatus { LogStatusId = 12, Name = "Service Request Resolved", LogTypeId = 5, IsActive = true },
                new LogStatus { LogStatusId = 13, Name = "Add Rejected", LogTypeId = 1, IsActive = true });

            modelBuilder.Entity<DisputeType>().HasData(
                new DisputeType { DisputeTypeId = 1, Name = "Dispute", IsActive = true },
                new DisputeType { DisputeTypeId = 2, Name = "Resolution", IsActive = true });

            modelBuilder.Entity<DisputeDiscrepancy>().HasData(
                new DisputeDiscrepancy { DisputeDiscrepancyId = 1, Name = "Billed hours discrepancy", IsActive = true },
                new DisputeDiscrepancy { DisputeDiscrepancyId = 2, Name = "Billed usage discrepancy", IsActive = true },
                new DisputeDiscrepancy { DisputeDiscrepancyId = 3, Name = "Service not provided discrepancy", IsActive = true },
                new DisputeDiscrepancy { DisputeDiscrepancyId = 4, Name = "Time period discrepancy (i.e. incorrect prorated dollar amount)", IsActive = true },
                new DisputeDiscrepancy { DisputeDiscrepancyId = 5, Name = "Other - please explain", IsActive = true });

            modelBuilder.ApplyConfiguration(new ExcelFailureDbConfiguration());
            modelBuilder.ApplyConfiguration(new RoleDbConfiguration());
            modelBuilder.ApplyConfiguration(new UserDbConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleDbConfiguration());

            modelBuilder.Entity<ChangeResolutionLog>()
                .Ignore(e => e.RequestType);

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1L, Name = "admin", Disabled = false },
                new Role { Id = 2L, Name = "tsaTeam", Disabled = false },
                new Role { Id = 3L, Name = "executive", Disabled = false },
                new Role { Id = 4L, Name = "primaryOwner", Disabled = false },
                new Role { Id = 5L, Name = "personnel", Disabled = false });

            modelBuilder.Entity<AddType>().HasData(
                new AddType { AddTypeId = 1, Name = "Add a New TSA", IsActive = true },
                new AddType { AddTypeId = 2, Name = "Add a New Sub-TSA to an existing TSA", IsActive = true },
                new AddType { AddTypeId = 3, Name = "Add a New Service Request for an existing TSA", IsActive = true });

            modelBuilder.Entity<VLogTypeStatus>(eb => {
                eb.ToView("v_log_type_status");
            });
            modelBuilder.Entity<VInputStreamSummary>(eb => {
                eb.ToView("v_input_stream_summary");
            });
            modelBuilder.Entity<VPaymentApprovalReport>(eb => {
                eb.ToView("v_payment_approval_report");
            });
        }
    }
}
