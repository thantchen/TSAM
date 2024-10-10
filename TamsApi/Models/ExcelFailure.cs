using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TamsApi.Data;

namespace TamsApi.Models
{
    public class ExcelFailure : ITrackCreated
    {
        public long Id { get; set; }

        [Required]
        public string Errors { get; set; }

        public string StackTrace { get; set; }

        [Required]
        [StringLength(256)]
        public string FileName { get; set; }

        [Required]
        [StringLength(256)]
        public string FileType { get; set; }

        public long? ExcelRow { get; set; }

        public IDictionary<string, object> CellData { get; set; }

        public DateTime CreatedDate { get; set; }
        public long CreatedUserId { get; set; }
        public User CreatedUser { get; set; }
    }

    public class ExcelFailureDbConfiguration : IEntityTypeConfiguration<ExcelFailure>
    {
        public void Configure(EntityTypeBuilder<ExcelFailure> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.FileName)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(e => e.FileType)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(e => e.Errors)
                .IsRequired()
                .HasColumnType("ntext");

            builder.Property(e => e.StackTrace)
                .HasColumnType("ntext");

            builder.Property(e => e.ExcelRow);

            builder.Property(e => e.CreatedDate)
                .HasColumnType("datetime");

            // This uses a key-only value comparer. There really is no
            // need to track changes to this property as it's only for logging purposes
            // and will not change.
            builder.Property(e => e.CellData)
                .HasColumnType("ntext")
                .HasConversion(DbValueConverters.JsonDictionaryConverter)
                .Metadata
                .SetValueComparer(DbValueComparers.KeyOnlyDictionaryComparer);

            builder.HasOne(e => e.CreatedUser)
                .WithMany()
                .HasForeignKey(e => e.CreatedUserId);
        }
    }
}
