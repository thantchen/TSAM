using System;
using System.Collections.Generic;

namespace TamsApi.Models
{
    public partial class DisputeType
    {
        public DisputeType()
        {
            DisputeLog = new HashSet<DisputeLog>();
        }

        public int DisputeTypeId { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<DisputeLog> DisputeLog { get; set; }
    }
}
