using System;
using System.ComponentModel.DataAnnotations;

namespace TamsApi.Models
{
    public class ExcelColumnMapping
    {
        public string DbColumn { get; set; }

        public string ExcelHeader { get; set; }

        public string CastType { get; set; }

        public bool IsMergedCell { get; set; }

        // Override to equal for EF core value comparison and change tracking.
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (!(obj is ExcelColumnMapping)) return false;

            return DbColumn.ToLowerInvariant() == ((ExcelColumnMapping) obj).DbColumn.ToLowerInvariant();
        }

        public override int GetHashCode()
        {
            if (string.IsNullOrWhiteSpace(DbColumn))
            {
                throw new InvalidOperationException("Cannot get hash code without DbColumn value");
            }

            return DbColumn.ToLowerInvariant().GetHashCode();
        }
    }
}
