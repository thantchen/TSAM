using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TamsApi.Data;

namespace TamsApi.Models
{
    public class Role
    {
        public Role()
        {
            UserRoles = new List<UserRole>();
        }

        public long Id { get; set; }

        [StringLength(256)]
        public string Name { get; set; }

        public bool Disabled { get; set; }

        public DateTime? DisabledAt { get; set; }

        public List<UserRole> UserRoles { get; set; }
    }

    public class RoleDbConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("role");

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(e => e.Disabled)
                .HasColumnName("disabled")
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(r => r.DisabledAt)
                .HasColumnName("disabled_at")
                .HasConversion(DbValueConverters.UtcDateTimeConverter);

            builder.HasIndex(r => r.Name).IsUnique();

            builder.HasMany(e => e.UserRoles)
                .WithOne(e => e.Role)
                .HasForeignKey(e => e.RoleId);
        }
    }
}
