using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace TamsApi.Models
{
    public class ExcelMapConfiguration : IEntityTypeConfiguration<ExcelMap>
    {
        public static JsonSerializerSettings JsonSettings = new JsonSerializerSettings
            {NullValueHandling = NullValueHandling.Ignore};


        public void Configure(EntityTypeBuilder<ExcelMap> builder)
        {
            builder.Property(e => e.DbContextType)
                .IsRequired()
                .HasConversion(
                    e => e.AssemblyQualifiedName,
                    e => Type.GetType(e, true));

            // Create value comparison to eliminate migration warning from EF Core.
            // https://github.com/dotnet/efcore/issues/17471#issuecomment-526330450
            var mappingComparer = new ValueComparer<ICollection<ExcelColumnMapping>>(
                (m1, m2) => m1.SequenceEqual(m2),
                m => m.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => (ICollection<ExcelColumnMapping>)c.ToHashSet());
            builder.Property(e => e.Mappings)
                .IsRequired()
                .HasConversion(
                    v => JsonConvert.SerializeObject(v, JsonSettings),
                    v => JsonConvert.DeserializeObject<IList<ExcelColumnMapping>>(v, JsonSettings))
                .Metadata.SetValueComparer(mappingComparer);
        }
    }
}
