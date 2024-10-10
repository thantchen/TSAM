using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TamsApi.Models
{
    public partial class FileRepository : ITrackCreated
    {
        public FileRepository()
        {
            AddLogFile = new HashSet<AddLogFile>();
            ChangeResolutionLog = new HashSet<ChangeResolutionLog>();
            DisputeLogFile = new HashSet<DisputeLogFile>();
            Invoice = new HashSet<Invoice>();
            Payment = new HashSet<Payment>();
            SellSideNotificationChange = new HashSet<SellSideNotificationChange>();
            SellSideNotificationDispute = new HashSet<SellSideNotificationDispute>();
            TsaSchedule = new HashSet<TsaSchedule>();
        }

        public int FileRepositoryId { get; set; }
        public string FileName { get; set; }
        public byte[] FileStream { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedUserId { get; set; }
        public int FileTypeId { get; set; }

        public virtual FileType FileType { get; set; }
        public virtual User CreatedUser { get; set; }
        public virtual ICollection<AddLogFile> AddLogFile { get; set; }
        public virtual ICollection<ChangeResolutionLog> ChangeResolutionLog { get; set; }
        public virtual ICollection<DisputeLogFile> DisputeLogFile { get; set; }
        public virtual ICollection<Invoice> Invoice { get; set; }
        public virtual ICollection<Payment> Payment { get; set; }
        public virtual ICollection<SellSideNotificationChange> SellSideNotificationChange { get; set; }
        public virtual ICollection<SellSideNotificationDispute> SellSideNotificationDispute { get; set; }
        public virtual ICollection<TsaSchedule> TsaSchedule { get; set; }
    }
}
