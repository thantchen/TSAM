using System;
using System.Collections.Generic;

namespace TamsApi.Models
{
    public partial class DisputeDiscrepancy
    {
        public DisputeDiscrepancy()
        {
            DisputeLog = new HashSet<DisputeLog>();
        }

        public int DisputeDiscrepancyId { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<DisputeLog> DisputeLog { get; set; }
    }
}
