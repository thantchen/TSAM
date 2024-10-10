using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TamsApi.Models
{
    public partial class SellSideNotificationChange : ITrackModified, ITsaChild
    {
        public int NotificationChangeId { get; set; }
        [StringLength(24)]
        public string TsaId { get; set; }

        [StringLength(24)]
        public string TsaSubId { get; set; }

        [StringLength(128)]
        public string PrimaryOwner { get; set; }

        [StringLength(1024)]
        public string ServiceName { get; set; }

        [StringLength(64)]
        public string Status { get; set; }

        [StringLength(512)]
        public string ActionItem { get; set; }

        public int FileRepositoryId { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public long LastModifiedUserId { get; set; }

        public virtual FileRepository FileRepository { get; set; }
        public virtual User LastModifiedUser { get; set; }
        public virtual TsaSchedule TsaSub { get; set; }
    }
}
