using System;
using System.Collections.Generic;

namespace TamsApi.Models
{
    public partial class LogType
    {
        public LogType()
        {
            AddLog = new HashSet<AddLog>();
            ChangeLog = new HashSet<ChangeLog>();
            ChangeResolutionLog = new HashSet<ChangeResolutionLog>();
            LogStatus = new HashSet<LogStatus>();
        }

        public int LogTypeId { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<AddLog> AddLog { get; set; }
        public virtual ICollection<ChangeLog> ChangeLog { get; set; }
        public virtual ICollection<ChangeResolutionLog> ChangeResolutionLog { get; set; }
        public virtual ICollection<LogStatus> LogStatus { get; set; }
    }
}
