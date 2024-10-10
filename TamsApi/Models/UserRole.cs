using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TamsApi.Models
{
    public class UserRole
    {
        public long Id { get; set; }

        public long RoleId { get; set; }

        public Role Role { get; set; }

        public long UserId { get; set; }

        public User User { get; set; }
    }

    public class UserRoleDbConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("user_role");

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.RoleId)
                .HasColumnName("role_id")
                .IsRequired();

            builder.Property(e => e.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.HasOne(e => e.Role)
                .WithMany(e => e.UserRoles)
                .HasForeignKey(e => e.RoleId);

            builder.HasOne(e => e.User)
                .WithMany(e => e.UserRoles)
                .HasForeignKey(e => e.UserId);

            builder.HasIndex(ur => new { ur.UserId, ur.RoleId })
                .IsUnique();
        }
    }
}
