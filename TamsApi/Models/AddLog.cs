using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TamsApi.Models
{
    public partial class AddLog : ITrackCreated, ITrackModified, ITrackRevisions
    {
        public AddLog()
        {
            AddLogFile = new HashSet<AddLogFile>();
        }

        public int AddLogId { get; set; }
        public string TsaId { get; set; }
        public string TsaSubId { get; set; }
        public DateTime? ChangeDate { get; set; }
        public int ChangeLogTypeId { get; set; }
        public int ChangeLogStatusId { get; set; }
        public long RequestedByUserId { get; set; }
        public User RequestedByUser { get; set; }
        public long SubmittedByUserId { get; set; }
        public User SubmittedByUser { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        [MaxLength]
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedUserId { get; set; }
        public User CreatedUser { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public long LastModifiedUserId { get; set; }
        public User LastModifiedUser { get; set; }
        public int Revision { get; set; }
        public string AddId { get; set; }

        public virtual LogStatus ChangeLogStatus { get; set; }
        public virtual LogType ChangeLogType { get; set; }

        public virtual TsaSchedule TsaSub { get; set; }
        public virtual ICollection<AddLogFile> AddLogFile { get; set; }
        public string TsaSubIds { get; set; }
        public int AddTypeId { get; set; }
        public virtual AddType AddType { get; set; }
    }
}
