using System;
using System.Collections.Generic;

namespace TamsApi.Models
{
    public partial class LogStatus
    {
        public LogStatus()
        {
            AddLog = new HashSet<AddLog>();
            ChangeLog = new HashSet<ChangeLog>();
        }

        public int LogStatusId { get; set; }
        public int? LogTypeId { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }

        public virtual LogType LogType { get; set; }
        public virtual ICollection<AddLog> AddLog { get; set; }
        public virtual ICollection<ChangeLog> ChangeLog { get; set; }
    }
}
