using System;
using System.Collections.Generic;

namespace TamsApi.Models
{
    public partial class AddType
    {
        public AddType()
        {
            AddLog = new HashSet<AddLog>();
        }

        public int AddTypeId { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<AddLog> AddLog { get; set; }
    }
}
