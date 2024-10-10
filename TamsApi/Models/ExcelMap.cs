using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TamsApi.Models
{
    public class ExcelMap
    {
        public int Id { get; set; }

        public Type DbContextType { get; set; }

        public string PrimaryKey { get; set; }

        public ICollection<ExcelColumnMapping> Mappings { get; set; }
    }
}
