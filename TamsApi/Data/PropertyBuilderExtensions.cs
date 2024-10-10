using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TamsApi.Data
{
    public static class PropertyBuilderExtensions
    {
        public static PropertyBuilder<decimal?> AsTamsCurrency(this PropertyBuilder<decimal?> propertyBuilder, string columnName)
        {
            return propertyBuilder.HasColumnType("decimal(18, 2)")
                .HasColumnName(columnName);
        }
    }
}
