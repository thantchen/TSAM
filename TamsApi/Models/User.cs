using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TamsApi.Data;

namespace TamsApi.Models
{
    public partial class User
    {
        public User()
        {
            UserRoles = new List<UserRole>();
        }

        public long Id { get; set; }

        [StringLength(256)]
        public string UserName { get; set; }

        [StringLength(256)]
        public string FirstName { get; set; }

        [StringLength(256)]
        public string LastName { get; set; }

        [StringLength(512)]
        public string Email { get; set; }

        public bool Disabled { get; set; }

        public DateTime? DisabledAt { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public List<UserRole> UserRoles { get; set; }
    }

    public class UserDbConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user");

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.UserName)
                .HasColumnName("user_name")
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(e => e.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(256);

            builder.Property(e => e.LastName)
                .HasColumnName("last_name")
                .HasMaxLength(256);

            builder.Property(e => e.Email)
                .HasColumnName("email")
                .HasMaxLength(512);

            builder.Property(e => e.Disabled)
                .HasColumnName("disabled")
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(r => r.DisabledAt)
                .HasColumnName("disabled_at")
                .HasConversion(DbValueConverters.UtcDateTimeConverter);

            builder.Property(r => r.CreatedDate)
                .HasColumnName("created_date")
                .HasConversion(DbValueConverters.UtcNullableDateTimeConverter);

            builder.Property(r => r.LastModifiedDate)
                .HasColumnName("last_modified_date")
                .HasConversion(DbValueConverters.UtcNullableDateTimeConverter);

            builder.HasIndex(r => r.UserName).IsUnique();

            builder.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);
        }
    }
}
