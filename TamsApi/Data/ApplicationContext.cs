using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TamsApi.Models;

namespace TamsApi.Data
{
    public partial class ApplicationContext : DbContext
    {
        public virtual DbSet<AddLog> AddLog { get; set; }
        public virtual DbSet<AddLogFile> AddLogFile { get; set; }
        public virtual DbSet<ChangeLog> ChangeLog { get; set; }
        public virtual DbSet<ChangeLogFile> ChangeLogFile { get; set; }
        public virtual DbSet<ChangeResolutionLog> ChangeResolutionLog { get; set; }
        public virtual DbSet<DisputeDiscrepancy> DisputeDiscrepancy { get; set; }
        public virtual DbSet<DisputeLog> DisputeLog { get; set; }
        public virtual DbSet<DisputeLogFile> DisputeLogFile { get; set; }
        public virtual DbSet<DisputeType> DisputeType { get; set; }
        public virtual DbSet<FileRepository> FileRepository { get; set; }
        public virtual DbSet<FileType> FileType { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<LogStatus> LogStatus { get; set; }
        public virtual DbSet<LogType> LogType { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<SellSideNotificationChange> SellSideNotificationChange { get; set; }
        public virtual DbSet<SellSideNotificationDispute> SellSideNotificationDispute { get; set; }
        public virtual DbSet<TsaSchedule> TsaSchedule { get; set; }
        public virtual DbSet<AddType> AddType { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AddLog>(entity =>
            {
                entity.ToTable("add_log");

                entity.HasIndex(e => e.TsaId)
                    .HasName("add_log__idxv1");

                entity.HasIndex(e => e.TsaSubId)
                    .HasName("add_log__idxv2");

                entity.Property(e => e.AddLogId).HasColumnName("add_log_id");

                entity.Property(e => e.ChangeDate)
                    .HasColumnName("change_date")
                    .HasColumnType("date");

                entity.Property(e => e.ChangeLogStatusId).HasColumnName("change_log_status_id");

                entity.Property(e => e.ChangeLogTypeId).HasColumnName("change_log_type_id");

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .HasColumnType("nvarchar(max)")
                    //.HasMaxLength(1024)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasColumnType("datetime")
                    .HasConversion(DbValueConverters.UtcDateTimeConverter);

                entity.Property(e => e.CreatedUserId)
                    .IsRequired()
                    .HasColumnName("created_user_id");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnName("last_modified_date")
                    .HasColumnType("datetime")
                    .HasConversion(DbValueConverters.UtcDateTimeConverter);

                entity.Property(e => e.LastModifiedUserId)
                    .IsRequired()
                    .HasColumnName("last_modified_user_id");

                entity.Property(e => e.RequestedByUserId)
                    .IsRequired()
                    .HasColumnName("requested_by_user_id");

                entity.Property(e => e.Revision).HasColumnName("revision");

                entity.Property(e => e.SubmittedByUserId)
                    .IsRequired()
                    .HasColumnName("submitted_by_user_id");

                entity.Property(e => e.TsaId)
                    .HasColumnName("tsa_id")
                    .HasMaxLength(24);

                entity.Property(e => e.TsaSubId)
                    .HasColumnName("tsa_sub_id")
                    .HasMaxLength(24);

                entity.Property(e => e.TsaSubIds)
                    .HasColumnName("tsa_sub_ids")
                    .HasMaxLength(128);

                entity.Property(e => e.AddTypeId).HasColumnName("add_type_id");

                entity.Property(e => e.AddId)
                    .HasColumnName("add_id")
                    .HasMaxLength(32);

                entity.HasOne(d => d.ChangeLogStatus)
                    .WithMany(p => p.AddLog)
                    .HasForeignKey(d => d.ChangeLogStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("add_log_log_status_fk");

                entity.HasOne(d => d.ChangeLogType)
                    .WithMany(p => p.AddLog)
                    .HasForeignKey(d => d.ChangeLogTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("add_log_log_type_fk");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany()
                    .HasForeignKey(d => d.CreatedUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.LastModifiedUser)
                    .WithMany()
                    .HasForeignKey(d => d.LastModifiedUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.RequestedByUser)
                    .WithMany()
                    .HasForeignKey(l => l.RequestedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.SubmittedByUser)
                    .WithMany()
                    .HasForeignKey(d => d.SubmittedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.TsaSub)
                    .WithMany(p => p.AddLog)
                    .HasForeignKey(d => d.TsaSubId)
                    .HasConstraintName("add_log_tsa_schedule_fk");

                entity.HasOne(d => d.AddType)
                    .WithMany(p => p.AddLog)
                    .HasForeignKey(d => d.AddTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("add_log_add_type_fk");
            });

            modelBuilder.Entity<AddLogFile>(entity =>
            {
                entity.ToTable("add_log_file");

                entity.Property(e => e.AddLogFileId).HasColumnName("add_log_file_id");

                entity.Property(e => e.AddLogId).HasColumnName("add_log_id");

                entity.Property(e => e.FileRepositoryId).HasColumnName("file_repository_id");

                entity.HasOne(d => d.AddLog)
                    .WithMany(p => p.AddLogFile)
                    .HasForeignKey(d => d.AddLogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("add_log_file_add_log_fk");

                entity.HasOne(d => d.FileRepository)
                    .WithMany(p => p.AddLogFile)
                    .HasForeignKey(d => d.FileRepositoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("add_log_file_file_repository_fk");
            });

            modelBuilder.Entity<ChangeLog>(entity =>
            {
                entity.ToTable("change_log");

                entity.HasIndex(e => e.TsaId)
                    .HasName("change_log__idxv1");

                entity.HasIndex(e => e.TsaSubId)
                    .HasName("change_log__idxv2");

                entity.Property(e => e.ChangeLogId).HasColumnName("change_log_id");

                entity.Property(e => e.ChangeDate)
                    .HasColumnName("change_date")
                    .HasColumnType("date");

                entity.Property(e => e.ChangeId)
                    .HasColumnName("change_id")
                    .HasMaxLength(32);

                entity.Property(e => e.ChangeLogStatusId).HasColumnName("change_log_status_id");

                entity.Property(e => e.ChangeLogTypeId).HasColumnName("change_log_type_id");

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .HasColumnType("nvarchar(max)")
                    //.HasMaxLength(1024)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasColumnType("datetime")
                    .HasConversion(DbValueConverters.UtcDateTimeConverter);

                entity.Property(e => e.CreatedUserId)
                    .IsRequired()
                    .HasColumnName("created_user_id");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnName("last_modified_date")
                    .HasColumnType("datetime")
                    .HasConversion(DbValueConverters.UtcDateTimeConverter);

                entity.Property(e => e.LastModifiedUserId)
                    .IsRequired()
                    .HasColumnName("last_modified_user_id");

                entity.Property(e => e.RequestedByUserId)
                    .IsRequired()
                    .HasColumnName("requested_by_user_id");

                entity.Property(e => e.Revision).HasColumnName("revision");

                entity.Property(e => e.SubmittedByUserId)
                    .IsRequired()
                    .HasColumnName("submitted_by_user_id");

                entity.Property(e => e.TsaId)
                    .HasColumnName("tsa_id")
                    .HasMaxLength(24);

                entity.Property(e => e.TsaSubId)
                    .IsRequired()
                    .HasColumnName("tsa_sub_id")
                    .HasMaxLength(24);

                entity.HasOne(d => d.ChangeLogStatus)
                    .WithMany(p => p.ChangeLog)
                    .HasForeignKey(d => d.ChangeLogStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("log_change_log_status_fk");

                entity.HasOne(d => d.ChangeLogType)
                    .WithMany(p => p.ChangeLog)
                    .HasForeignKey(d => d.ChangeLogTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("log_change_log_type_fk");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany()
                    .HasForeignKey(d => d.CreatedUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.LastModifiedUser)
                    .WithMany()
                    .HasForeignKey(d => d.LastModifiedUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.RequestedByUser)
                    .WithMany()
                    .HasForeignKey(d => d.RequestedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.SubmittedByUser)
                    .WithMany()
                    .HasForeignKey(d => d.SubmittedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.TsaSub)
                    .WithMany(p => p.ChangeLog)
                    .HasForeignKey(d => d.TsaSubId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ChangeLogFile>(entity =>
            {
                entity.ToTable("change_log_file");

                entity.Property(e => e.ChangeLogFileId)
                    .HasColumnName("change_log_file_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ChangeLogId).HasColumnName("change_log_id");

                entity.Property(e => e.FileRepositoryId).HasColumnName("file_repository_id");

                entity.HasOne(d => d.ChangeLog)
                    .WithMany()
                    .HasForeignKey(d => d.ChangeLogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("change_log_file_change_log_fk");

                entity.HasOne(d => d.FileRepository)
                    .WithMany()
                    .HasForeignKey(d => d.FileRepositoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("change_log_file_file_repository_fk");
            });

            modelBuilder.Entity<ChangeResolutionLog>(entity =>
            {
                entity.ToTable("change_resolution_log");

                entity.HasIndex(e => e.TsaId)
                    .HasName("change_resolution_log__idxv1");

                entity.HasIndex(e => e.TsaSubId)
                    .HasName("change_resolution_log__idxv2");

                entity.Property(e => e.ChangeResolutionLogId)
                    .HasColumnName("change_resolution_log_id");

                entity.Property(e => e.ActiveOptionDurationExercised).HasColumnName("active_option_duration_exercised");

                entity.Property(e => e.AdditionalPricingComments)
                    .HasColumnName("additional_pricing_comments")
                    .HasMaxLength(1024);

                entity.Property(e => e.Assumptions)
                    .HasColumnName("assumptions")
                    .HasColumnType("ntext");

                entity.Property(e => e.ChangeId)
                    .HasColumnName("change_id")
                    .HasMaxLength(32);

                entity.Property(e => e.ChangeLogTypeId).HasColumnName("change_log_type_id");

                entity.Property(e => e.CostType)
                    .HasColumnName("cost_type")
                    .HasMaxLength(128);

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasColumnType("datetime")
                    .HasConversion(DbValueConverters.UtcDateTimeConverter);

                entity.Property(e => e.CreatedUserId)
                    .IsRequired()
                    .HasColumnName("created_user_id");

                entity.Property(e => e.DetailedServiceDescription)
                    .HasColumnName("detailed_service_description")
                    .HasColumnType("ntext");

                entity.Property(e => e.EffectiveDate)
                    .HasColumnName("effective_date")
                    .HasColumnType("date");

                entity.Property(e => e.EstimatedDurationText)
                    .HasColumnName("estimated_duration_text")
                    .HasMaxLength(64);

                entity.Property(e => e.ExceptionsReOwnershipIp)
                    .HasColumnName("exceptions_re_ownership_ip")
                    .HasMaxLength(256);

                entity.Property(e => e.ExceptionsTo30DayPayment)
                    .HasColumnName("exceptions_to_30_day_payment")
                    .HasMaxLength(256);

                entity.Property(e => e.ExceptionsToMonthlyInvoice)
                    .HasColumnName("exceptions_to_monthly_invoice")
                    .HasMaxLength(512);

                entity.Property(e => e.FileRepositoryId).HasColumnName("file_repository_id");

                entity.Property(e => e.Function)
                    .HasColumnName("function")
                    .HasMaxLength(128);

                entity.Property(e => e.KnownDependencies)
                    .HasColumnName("known_dependencies")
                    .HasMaxLength(1024);

                entity.Property(e => e.KnownTsaExitCosts)
                    .HasColumnName("known_tsa_exit_costs")
                    .HasMaxLength(512);

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnName("last_modified_date")
                    .HasColumnType("datetime")
                    .HasConversion(DbValueConverters.UtcDateTimeConverter);

                entity.Property(e => e.LastModifiedUserId)
                    .IsRequired()
                    .HasColumnName("last_modified_user_id");

                entity.Property(e => e.MonthlyCostForecast)
                    .HasColumnName("monthly_cost_forecast")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.NoticeOfTerminationText)
                    .HasColumnName("notice_of_termination_text")
                    .HasMaxLength(128);

                entity.Property(e => e.NumOfUnits)
                    .HasColumnName("num_of_units")
                    .HasMaxLength(128);

                entity.Property(e => e.OptionExercisedFlag)
                    .HasColumnName("option_exercised_flag")
                    .HasMaxLength(24);

                entity.Property(e => e.PricePerUnit)
                    .HasColumnName("price_per_unit")
                    .HasMaxLength(128);

                entity.Property(e => e.ProviderOwner)
                    .HasColumnName("provider_owner")
                    .HasMaxLength(640);

                entity.Property(e => e.ReceiverOwner)
                    .HasColumnName("receiver_owner")
                    .HasMaxLength(640);

                entity.Property(e => e.ServiceExceptions)
                    .HasColumnName("service_exceptions")
                    .HasMaxLength(1024);

                entity.Property(e => e.ServiceLocation)
                    .HasColumnName("service_location")
                    .HasMaxLength(512);

                entity.Property(e => e.ServiceName)
                    .HasColumnName("service_name")
                    .HasMaxLength(512);

                entity.Property(e => e.SubFunction)
                    .HasColumnName("sub_function")
                    .HasMaxLength(128);

                entity.Property(e => e.TaxDetermination)
                    .HasColumnName("tax_determination")
                    .HasMaxLength(256);

                entity.Property(e => e.TsaExitApproach)
                    .HasColumnName("tsa_exit_approach")
                    .HasMaxLength(512);

                entity.Property(e => e.TsaId)
                    .HasColumnName("tsa_id")
                    .HasMaxLength(24);

                entity.Property(e => e.TsaSubId)
                    .IsRequired()
                    .HasColumnName("tsa_sub_id")
                    .HasMaxLength(24);

                entity.Property(e => e.TsaVsRtsa)
                    .HasColumnName("tsa_vs_rtsa")
                    .HasMaxLength(16);

                entity.Property(e => e.UnitDescription)
                    .HasColumnName("unit_description")
                    .HasMaxLength(1024);

                entity.Property(e => e.PrimaryOwner)
                    .HasColumnName("primary_owner")
                    .HasMaxLength(640);

                entity.Property(e => e.OptionFlag)
                    .HasColumnName("option_flag")
                    .HasMaxLength(8);

                entity.Property(e => e.MaxOptionDuration).HasColumnName("max_option_duration");

                entity.Property(e => e.ExitCostAmount)
                    .HasColumnName("exit_cost_amount")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ActiveEndDate)
                    .HasColumnName("active_end_date")
                    .HasColumnType("date");

                entity.Property(e => e.Currency)
                    .HasColumnName("currency")
                    .HasMaxLength(3);

                entity.Property(e => e.SeparationOrStandalone)
                    .HasColumnName("separation_or_standalone")
                    .HasMaxLength(32);

                entity.Property(e => e.NoticeOfTermNum).HasColumnName("notice_of_term_num");

                entity.HasOne(d => d.ChangeLogType)
                    .WithMany(p => p.ChangeResolutionLog)
                    .HasForeignKey(d => d.ChangeLogTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("change_resolution_log_change_request_type_fk");

                entity.Property(d => d.MonthlyPricing)
                    .HasColumnName("monthly_pricing")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(d => d.MonthlyPricingText)
                    .HasColumnName("monthly_pricing_text")
                    .HasMaxLength(32);

                entity.HasOne(d => d.CreatedUser)
                    .WithMany()
                    .HasForeignKey(d => d.CreatedUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.FileRepository)
                    .WithMany(p => p.ChangeResolutionLog)
                    .HasForeignKey(d => d.FileRepositoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tsa_schedulev1_file_repository_fk");

                entity.HasOne(d => d.LastModifiedUser)
                    .WithMany()
                    .HasForeignKey(d => d.LastModifiedUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.TsaSub)
                    .WithMany(p => p.ChangeResolutionLog)
                    .HasForeignKey(d => d.TsaSubId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("change_resolution_log_tsa_schedule_fk");
            });

            modelBuilder.Entity<DisputeDiscrepancy>(entity =>
            {
                entity.ToTable("dispute_discrepancy");

                entity.Property(e => e.DisputeDiscrepancyId).HasColumnName("dispute_discrepancy_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<DisputeLog>(entity =>
            {
                entity.ToTable("dispute_log");

                entity.HasIndex(e => e.TsaId)
                    .HasName("dispute_log__idxv1");

                entity.HasIndex(e => e.TsaSubId)
                    .HasName("dispute_log__idxv2");

                entity.Property(e => e.DisputeLogId).HasColumnName("dispute_log_id");

                entity.Property(e => e.AgreedCost)
                    .HasColumnName("agreed_cost")
                    .HasColumnType("money");

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .HasColumnType("nvarchar(max)");
                    //.HasMaxLength(1028);

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasColumnType("datetime")
                    .HasConversion(DbValueConverters.UtcDateTimeConverter);

                entity.Property(e => e.CreatedUserId)
                    .IsRequired()
                    .HasColumnName("created_user_id");

                entity.Property(e => e.DisputeDiscrepancyId).HasColumnName("dispute_discrepancy_id");

                entity.Property(e => e.DisputeId)
                    .HasColumnName("dispute_id")
                    .HasMaxLength(32);

                entity.Property(e => e.DisputeTypeId).HasColumnName("dispute_type_id");

                entity.Property(e => e.InvoiceNumber)
                    .HasColumnName("invoice_number")
                    .HasMaxLength(24);

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnName("last_modified_date")
                    .HasColumnType("datetime")
                    .HasConversion(DbValueConverters.UtcDateTimeConverter);

                entity.Property(e => e.LastModifiedUserId)
                    .IsRequired()
                    .HasColumnName("last_modified_user_id");

                entity.Property(e => e.RequestedByUserId)
                    .IsRequired()
                    .HasColumnName("requested_by_user_id");

                entity.Property(e => e.Revision).HasColumnName("revision");

                entity.Property(e => e.SubmissionDate)
                    .HasColumnName("submission_date")
                    .HasColumnType("date");

                entity.Property(e => e.SubmittedByUserId)
                    .IsRequired()
                    .HasColumnName("submitted_by_user_id");

                entity.Property(e => e.TsaId)
                    .HasColumnName("tsa_id")
                    .HasMaxLength(24);

                entity.Property(e => e.TsaSubId)
                    .IsRequired()
                    .HasColumnName("tsa_sub_id")
                    .HasMaxLength(24);

                entity.HasOne(p => p.CreatedUser)
                    .WithMany()
                    .HasForeignKey(d => d.CreatedUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.DisputeDiscrepancy)
                    .WithMany(p => p.DisputeLog)
                    .HasForeignKey(d => d.DisputeDiscrepancyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("dispute_log_dispute_discrepancy_fk");

                entity.HasOne(d => d.DisputeType)
                    .WithMany(p => p.DisputeLog)
                    .HasForeignKey(d => d.DisputeTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("dispute_log_dispute_type_fk");

                entity.HasOne(d => d.LastModifiedUser)
                    .WithMany()
                    .HasForeignKey(d => d.LastModifiedUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.RequestedByUser)
                    .WithMany()
                    .HasForeignKey(d => d.RequestedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.SubmittedByUser)
                    .WithMany()
                    .HasForeignKey(d => d.SubmittedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.TsaSub)
                    .WithMany(p => p.DisputeLog)
                    .HasForeignKey(d => d.TsaSubId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("dispute_log_tsa_schedule_fk");

                entity.Property(e => e.Currency)
                    .HasColumnName("currency")
                    .HasMaxLength(3);

                entity.Property(e => e.ServicePeriod)
                    .HasColumnName("service_period")
                    .HasColumnType("date");
            });

            modelBuilder.Entity<DisputeLogFile>(entity =>
            {
                entity.ToTable("dispute_log_file");

                entity.Property(e => e.DisputeLogFileId).HasColumnName("dispute_log_file_id");

                entity.Property(e => e.DisputeLogId).HasColumnName("dispute_log_id");

                entity.Property(e => e.FileRepositoryId).HasColumnName("file_repository_id");

                entity.HasOne(d => d.DisputeLog)
                    .WithMany(p => p.DisputeLogFile)
                    .HasForeignKey(d => d.DisputeLogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("dispute_request_file_dispute_log_fk");

                entity.HasOne(d => d.FileRepository)
                    .WithMany(p => p.DisputeLogFile)
                    .HasForeignKey(d => d.FileRepositoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("dispute_request_file_file_repository_fk");
            });

            modelBuilder.Entity<DisputeType>(entity =>
            {
                entity.ToTable("dispute_type");

                entity.Property(e => e.DisputeTypeId).HasColumnName("dispute_type_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<FileRepository>(entity =>
            {
                entity.ToTable("file_repository");

                entity.Property(e => e.FileRepositoryId).HasColumnName("file_repository_id");

                entity.Property(e => e.FileName)
                    .HasColumnName("file_name")
                    .HasMaxLength(256);

                entity.Property(e => e.FileStream)
                    .IsRequired()
                    .HasColumnName("file_stream")
                    .HasColumnType("varbinary(max)");

                entity.Property(e => e.FileTypeId).HasColumnName("file_type_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasColumnType("datetime")
                    .HasConversion(DbValueConverters.UtcDateTimeConverter);

                entity.Property(e => e.CreatedUserId)
                    .IsRequired()
                    .HasColumnName("created_user_id");

                entity.HasOne(d => d.FileType)
                    .WithMany(p => p.FileRepository)
                    .HasForeignKey(d => d.FileTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("file_repository_file_type_fk");

                entity.HasOne(e => e.CreatedUser)
                    .WithMany()
                    .HasForeignKey(d => d.CreatedUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<FileType>(entity =>
            {
                entity.ToTable("file_type");

                entity.Property(e => e.FileTypeId).HasColumnName("file_type_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(32)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.ToTable("invoice");

                entity.HasIndex(e => e.TsaId)
                    .HasName("invoice__idxv1");

                entity.HasIndex(e => e.TsaSubId)
                    .HasName("invoice__idxv2");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Cost)
                    .HasColumnName("cost")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Markup)
                    .HasColumnName("markup")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InvoiceAmount)
                    .HasColumnName("invoice_amount")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasColumnType("datetime")
                    .HasConversion(DbValueConverters.UtcDateTimeConverter);

                entity.Property(e => e.CreatedUserId)
                    .IsRequired()
                    .HasColumnName("created_user_id");

                entity.Property(e => e.FileRepositoryId).HasColumnName("file_repository_id");

                entity.Property(e => e.InvoiceDate)
                    .HasColumnName("invoice_date")
                    .HasColumnType("date");

                entity.Property(e => e.InvoiceDueDate)
                    .HasColumnName("invoice_due_date")
                    .HasColumnType("date");

                entity.Property(e => e.InvoiceFrequency)
                    .HasColumnName("invoice_frequency")
                    .HasMaxLength(128);

                entity.Property(e => e.InvoiceNumber)
                    .HasColumnName("invoice_number")
                    .HasMaxLength(24);

                entity.Property(e => e.InvoicePeriodEndDate)
                    .HasColumnName("invoice_period_end_date")
                    .HasColumnType("date");

                entity.Property(e => e.InvoicePeriodStartDate)
                    .HasColumnName("invoice_period_start_date")
                    .HasColumnType("date");

                entity.Property(e => e.ServicePeriod)
                    .HasColumnName("service_period")
                    .HasColumnType("date");

                entity.Property(e => e.TsaId)
                    .HasColumnName("tsa_id")
                    .HasMaxLength(24);

                entity.Property(e => e.TsaSubId)
                    .IsRequired()
                    .HasColumnName("tsa_sub_id")
                    .HasMaxLength(24);

                entity.HasOne(d => d.CreatedUser)
                    .WithMany()
                    .HasForeignKey(d => d.CreatedUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.FileRepository)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.FileRepositoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("invoice_file_repository_fkv2");

                entity.HasOne(d => d.TsaSub)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.TsaSubId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("invoice_tsa_schedule_fk");

                entity.Property(e => e.Currency)
                    .HasColumnName("currency")
                    .HasMaxLength(3);

                entity.Property(e => e.UsdExchangeRate)
                    .HasColumnName("usd_exchange_rate")
                    .HasColumnType("decimal(18, 6)");

                entity.Property(e => e.CostUsd)
                    .AsTamsCurrency("cost_usd");

                entity.Property(e => e.MarkupUsd)
                    .AsTamsCurrency("markup_usd");

                entity.Property(e => e.InvoiceAmountUsd)
                    .AsTamsCurrency("invoice_amount_usd");

                entity.Property(e => e.UnitOfMeasure)
                    .HasColumnName("unit_of_measure")
                    .HasMaxLength(128);

                entity.Property(e => e.NumberOfUnits)
                     .HasColumnName("number_of_units")
                     .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ReferenceInvoiceNumber)
                    .HasColumnName("reference_invoice_number")
                    .HasMaxLength(24);
            });

            modelBuilder.Entity<LogStatus>(entity =>
            {
                entity.ToTable("log_status");

                entity.Property(e => e.LogStatusId).HasColumnName("log_status_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.LogTypeId).HasColumnName("log_type_id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(128);

                entity.HasOne(d => d.LogType)
                    .WithMany(p => p.LogStatus)
                    .HasForeignKey(d => d.LogTypeId)
                    .HasConstraintName("log_status_log_type_fk");
            });

            modelBuilder.Entity<LogType>(entity =>
            {
                entity.ToTable("log_type");

                entity.Property(e => e.LogTypeId).HasColumnName("log_type_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("payment");

                entity.HasIndex(e => e.TsaId)
                    .HasName("payment__idxv1");

                entity.HasIndex(e => e.TsaSubId)
                    .HasName("payment__idxv2");

                entity.Property(e => e.PaymentId).HasColumnName("payment_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasColumnType("datetime")
                    .HasConversion(DbValueConverters.UtcDateTimeConverter);

                entity.Property(e => e.CreatedUserId)
                    .IsRequired()
                    .HasColumnName("created_user_id");

                entity.Property(e => e.FileRepositoryId).HasColumnName("file_repository_id");

                entity.Property(e => e.InvoiceNumber)
                    .HasColumnName("invoice_number")
                    .HasMaxLength(24);

                entity.Property(e => e.PaymentAmount)
                    .HasColumnName("payment_amount")
                    .HasColumnType("numeric(18, 2)");

                entity.Property(e => e.PaymentDate)
                    .HasColumnName("payment_date")
                    .HasColumnType("date");

                entity.Property(e => e.PaymentType)
                    .HasColumnName("payment_type")
                    .HasMaxLength(128);

                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .HasMaxLength(256);

                entity.Property(e => e.TsaId)
                    .HasColumnName("tsa_id")
                    .HasMaxLength(24);

                entity.Property(e => e.TsaSubId)
                    .IsRequired()
                    .HasColumnName("tsa_sub_id")
                    .HasMaxLength(24);

                entity.HasOne(e => e.CreatedUser)
                    .WithMany()
                    .HasForeignKey(e => e.CreatedUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.FileRepository)
                    .WithMany(p => p.Payment)
                    .HasForeignKey(d => d.FileRepositoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("payment_file_repository_fkv2");

                entity.HasOne(d => d.TsaSub)
                    .WithMany(p => p.Payment)
                    .HasForeignKey(d => d.TsaSubId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("payment_tsa_schedule_fk");

                entity.Property(e => e.Currency)
                    .HasColumnName("currency")
                    .HasMaxLength(3);

                entity.Property(e => e.ExchangeRate)
                    .HasColumnName("exchange_rate")
                    .HasColumnType("decimal(18, 6)");

                entity.Property(e => e.BankAccountFrom)
                    .HasColumnName("bank_account_from")
                    .HasMaxLength(256);

                entity.Property(e => e.BankAccountTo)
                    .HasColumnName("bank_account_To")
                    .HasMaxLength(256);

                entity.Property(e => e.UsdConversion)
                    .HasColumnName("usd_conversion")
                    .HasColumnType("numeric(18, 2)");
            });

            modelBuilder.Entity<SellSideNotificationChange>(entity =>
            {
                entity.HasKey(e => e.NotificationChangeId)
                    .HasName("sell_side_notification_change_pk");

                entity.ToTable("sell_side_notification_change");

                entity.HasIndex(e => e.TsaId)
                    .HasName("sell_side_notification_change__idxv1");

                entity.HasIndex(e => e.TsaSubId)
                    .HasName("sell_side_notification_change__idxv2");

                entity.Property(e => e.NotificationChangeId).HasColumnName("notification_change_id");

                entity.Property(e => e.ActionItem)
                    .HasColumnName("action_item")
                    .HasMaxLength(512);

                entity.Property(e => e.FileRepositoryId).HasColumnName("file_repository_id");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnName("last_modified_date")
                    .HasColumnType("datetime")
                    .HasConversion(DbValueConverters.UtcDateTimeConverter);

                entity.Property(e => e.LastModifiedUserId)
                    .IsRequired()
                    .HasColumnName("last_modified_user_id");

                entity.Property(e => e.PrimaryOwner)
                    .HasColumnName("primary_owner")
                    .HasMaxLength(128);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(64);

                entity.Property(e => e.ServiceName)
                    .HasColumnName("service_name")
                    .HasMaxLength(1024);

                entity.Property(e => e.TsaId)
                    .HasColumnName("tsa_id")
                    .HasMaxLength(24);

                entity.Property(e => e.TsaSubId)
                    .IsRequired()
                    .HasColumnName("tsa_sub_id")
                    .HasMaxLength(24);

                entity.HasOne(d => d.FileRepository)
                    .WithMany(p => p.SellSideNotificationChange)
                    .HasForeignKey(d => d.FileRepositoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("sell_side_notification_change_file_repository_fk");

                entity.HasOne(e => e.LastModifiedUser)
                    .WithMany()
                    .HasForeignKey(d => d.LastModifiedUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.TsaSub)
                    .WithMany(p => p.SellSideNotificationChange)
                    .HasForeignKey(d => d.TsaSubId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("sell_side_notification_change_tsa_schedule_fk");
            });

            modelBuilder.Entity<SellSideNotificationDispute>(entity =>
            {
                entity.HasKey(e => e.NotificationDisputeId)
                    .HasName("sell_side_notification_dispute_pk");

                entity.ToTable("sell_side_notification_dispute");

                entity.HasIndex(e => e.TsaId)
                    .HasName("sell_side_notification_dispute__idxv1");

                entity.HasIndex(e => e.TsaSubId)
                    .HasName("sell_side_notification_dispute__idxv2");

                entity.Property(e => e.NotificationDisputeId).HasColumnName("notification_dispute_id");

                entity.Property(e => e.ActionItem)
                    .HasColumnName("action_item")
                    .HasMaxLength(512);

                entity.Property(e => e.FileRepositoryId).HasColumnName("file_repository_id");

                entity.Property(e => e.InvoiceAmount)
                    .HasColumnName("invoice_amount")
                    .HasColumnType("money");

                entity.Property(e => e.InvoiceCurrency)
                    .HasColumnName("invoice_currency")
                    .HasMaxLength(3);

                entity.Property(e => e.InvoiceNumber)
                    .HasColumnName("invoice_number")
                    .HasMaxLength(24);

                entity.Property(e => e.InvoicePeriodStartDate)
                    .HasColumnName("invoice_period_start_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.InvoicePeriodEndDate)
                    .HasColumnName("invoice_period_end_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.LastModifiedUserId)
                    .IsRequired()
                    .HasColumnName("last_modified_user_id");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnName("last_modified_date")
                    .HasColumnType("datetime")
                    .HasConversion(DbValueConverters.UtcDateTimeConverter);

                entity.Property(e => e.PrimaryOwner)
                    .HasColumnName("primary_owner")
                    .HasMaxLength(128);

                entity.Property(e => e.ServiceName)
                    .HasColumnName("service_name")
                    .HasMaxLength(1024);

                entity.Property(e => e.TsaId)
                    .HasColumnName("tsa_id")
                    .HasMaxLength(24);

                entity.Property(e => e.TsaSubId)
                    .IsRequired()
                    .HasColumnName("tsa_sub_id")
                    .HasMaxLength(24);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(64);

                entity.HasOne(d => d.FileRepository)
                    .WithMany(p => p.SellSideNotificationDispute)
                    .HasForeignKey(d => d.FileRepositoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("sell_side_notification_dispute_file_repository_fk");

                entity.HasOne(e => e.LastModifiedUser)
                    .WithMany()
                    .HasForeignKey(d => d.LastModifiedUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.TsaSub)
                    .WithMany(p => p.SellSideNotificationDispute)
                    .HasForeignKey(d => d.TsaSubId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("sell_side_notification_dispute_tsa_schedule_fk");
            });

            modelBuilder.Entity<TsaSchedule>(entity =>
            {
                entity.HasKey(e => e.TsaSubId)
                    .HasName("tsa_pk");

                entity.ToTable("tsa_schedule");

                entity.HasIndex(e => e.TsaId)
                    .HasName("tsa_schedule__idxv1");

                entity.Property(e => e.TsaSubId)
                    .HasColumnName("tsa_sub_id")
                    .HasMaxLength(24);

                entity.Property(e => e.ActiveEndDate)
                    .HasColumnName("active_end_date")
                    .HasColumnType("date");

                entity.Property(e => e.ActiveOptionDurationExercised).HasColumnName("active_option_duration_exercised");

                entity.Property(e => e.AdditionalPricingComments)
                    .HasColumnName("additional_pricing_comments")
                    .HasMaxLength(1024);

                entity.Property(e => e.Assumptions)
                    .HasColumnName("assumptions")
                    .HasColumnType("ntext");

                entity.Property(e => e.Bgn)
                    .HasColumnName("BGN")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Brl)
                    .HasColumnName("BRL")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ChangeLogType)
                    .HasColumnName("change_log_type")
                    .HasMaxLength(24);

                entity.Property(e => e.CostType)
                    .HasColumnName("cost_type")
                    .HasMaxLength(128);

                entity.Property(e => e.Crc)
                    .HasColumnName("CRC")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasColumnType("datetime")
                    .HasConversion(DbValueConverters.UtcDateTimeConverter);

                entity.Property(e => e.CreatedUserId)
                    .IsRequired()
                    .HasColumnName("created_user_id");

                entity.Property(e => e.DetailedServiceDescription)
                    .HasColumnName("detailed_service_description")
                    .HasColumnType("ntext");

                entity.Property(e => e.EstimatedDuration).HasColumnName("estimated_duration");

                entity.Property(e => e.EstimatedDurationText)
                    .HasColumnName("estimated_duration_text")
                    .HasMaxLength(64);

                entity.Property(e => e.ExceptionsReOwnershipIp)
                    .HasColumnName("exceptions_re_ownership_ip")
                    .HasMaxLength(256);

                entity.Property(e => e.ExceptionsTo30DayPayment)
                    .HasColumnName("exceptions_to_30_day_payment")
                    .HasMaxLength(256);

                entity.Property(e => e.ExceptionsToMonthlyInvoice)
                    .HasColumnName("exceptions_to_monthly_invoice")
                    .HasMaxLength(512);

                entity.Property(e => e.ExitCostAmount)
                    .HasColumnName("exit_cost_amount")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FileRepositoryId).HasColumnName("file_repository_id");

                entity.Property(e => e.Function)
                    .HasColumnName("function")
                    .HasMaxLength(128);

                entity.Property(e => e.Inr)
                    .HasColumnName("INR")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.KnownDependencies)
                    .HasColumnName("known_dependencies")
                    .HasMaxLength(1024);

                entity.Property(e => e.KnownTsaExitCosts)
                    .HasColumnName("known_tsa_exit_costs")
                    .HasMaxLength(512);

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnName("last_modified_date")
                    .HasColumnType("datetime")
                    .HasConversion(DbValueConverters.UtcDateTimeConverter);

                entity.Property(e => e.LastModifiedUserId)
                    .IsRequired()
                    .HasColumnName("last_modified_user_id");

                entity.Property(e => e.MaxOptionDuration).HasColumnName("max_option_duration");

                entity.Property(e => e.MonthlyCostForecast)
                    .HasColumnName("monthly_cost_forecast")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.MonthlyPricing)
                    .HasColumnName("monthly_pricing")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Mxn)
                    .HasColumnName("MXN")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.NoticeOfTermNum).HasColumnName("notice_of_term_num");

                entity.Property(e => e.NoticeOfTerminationText)
                    .HasColumnName("notice_of_termination_text")
                    .HasMaxLength(128);

                entity.Property(e => e.NumOfUnits)
                    .HasColumnName("num_of_units")
                    .HasMaxLength(128);

                entity.Property(e => e.OptionExercisedFlag)
                    .HasColumnName("option_exercised_flag")
                    .HasMaxLength(24);

                entity.Property(e => e.OptionFlag)
                      .HasColumnName("option_flag")
                      .HasMaxLength(8);

                entity.Property(e => e.OriginalEndDate)
                    .HasColumnName("original_end_date")
                    .HasColumnType("date");

                entity.Property(e => e.Php)
                    .HasColumnName("PHP")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PricePerUnit)
                    .HasColumnName("price_per_unit")
                    .HasMaxLength(128);

                entity.Property(e => e.PrimaryOwner)
                    .HasColumnName("primary_owner")
                    .HasMaxLength(640);

                entity.Property(e => e.ReceiverOwner)
                    .HasColumnName("receiver_owner")
                    .HasMaxLength(640);

                entity.Property(e => e.Revision).HasColumnName("revision");

                entity.Property(e => e.ServiceExceptions)
                    .HasColumnName("service_exceptions")
                    .HasMaxLength(1024);

                entity.Property(e => e.ServiceLocation)
                    .HasColumnName("service_location")
                    .HasMaxLength(512);

                entity.Property(e => e.ServiceName)
                    .HasColumnName("service_name")
                    .HasMaxLength(512);

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.SubFunction)
                    .HasColumnName("sub_function")
                    .HasMaxLength(128);

                entity.Property(e => e.TaxDetermination)
                    .HasColumnName("tax_determination")
                    .HasMaxLength(256);

                entity.Property(e => e.TsaExitApproach)
                    .HasColumnName("tsa_exit_approach")
                    .HasMaxLength(512);

                entity.Property(e => e.TsaId)
                    .HasColumnName("tsa_id")
                    .HasMaxLength(24);

                entity.Property(e => e.TsaVsRtsa)
                    .HasColumnName("tsa_vs_rtsa")
                    .HasMaxLength(16);

                entity.Property(e => e.UnitDescription)
                    .HasColumnName("unit_description")
                    .HasMaxLength(1024);

                entity.Property(e => e.Usd)
                    .HasColumnName("USD")
                    .HasColumnType("decimal(18, 2)");

                entity.HasOne(e => e.CreatedUser)
                    .WithMany()
                    .HasForeignKey(d => d.CreatedUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.FileRepository)
                    .WithMany(p => p.TsaSchedule)
                    .HasForeignKey(d => d.FileRepositoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tsa_schedule_file_repository_fk");

                entity.HasOne(e => e.LastModifiedUser)
                    .WithMany()
                    .HasForeignKey(d => d.LastModifiedUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Cad)
                    .AsTamsCurrency("CAD");

                entity.Property(e => e.Eur)
                    .AsTamsCurrency("EUR");

                entity.Property(e => e.Gbp)
                    .AsTamsCurrency("GBP");

                entity.Property(e => e.Dkk)
                    .AsTamsCurrency("DKK");

                entity.Property(e => e.Pln)
                    .AsTamsCurrency("PLN");

                entity.Property(e => e.Cny)
                    .AsTamsCurrency("CNY");

                entity.Property(e => e.Huf)
                    .AsTamsCurrency("HUF");

                entity.Property(e => e.Sgd)
                    .AsTamsCurrency("SGD");

                entity.Property(e => e.Aud)
                    .AsTamsCurrency("AUD");

                entity.Property(e => e.Czk)
                    .AsTamsCurrency("CZK");

                entity.Property(e => e.Currency)
                    .HasColumnName("currency")
                    .HasMaxLength(3);

                entity.Property(e => e.SeparationOrStandalone)
                    .HasColumnName("separation_or_standalone")
                    .HasMaxLength(32);

                entity.Property(e => e.ProviderOwner)
                    .HasColumnName("provider_owner")
                    .HasMaxLength(640);

                entity.Property(e => e.MonthlyPricingText)
                    .HasColumnName("monthly_pricing_text")
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<AddType>(entity =>
            {
                entity.ToTable("add_type");

                entity.Property(e => e.AddTypeId).HasColumnName("add_type_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(128);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
