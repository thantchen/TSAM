using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TamsApi.Models
{
    public partial class SellSideNotificationDispute : ITrackModified, ITsaChild
    {
        public int NotificationDisputeId { get; set; }

        [StringLength(24)]
        public string TsaId { get; set; }

        [StringLength(24)]
        public string TsaSubId { get; set; }

        [StringLength(128)]
        public string PrimaryOwner { get; set; }

        [StringLength(1024)]
        public string ServiceName { get; set; }

        [StringLength(64)]
        public string Type { get; set; }

        [StringLength(24)]
        public string InvoiceNumber { get; set; }

        [Range(-9999999999999999.99, 9999999999999999.99)]
        public decimal? InvoiceAmount { get; set; }

        [StringLength(512)]
        public string ActionItem { get; set; }
        public DateTime? InvoicePeriodStartDate { get; set; }
        public DateTime? InvoicePeriodEndDate { get; set; }

        [StringLength(3)]
        public string InvoiceCurrency { get; set; }
        public int FileRepositoryId { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public long LastModifiedUserId { get; set; }

        public virtual FileRepository FileRepository { get; set; }
        public virtual User LastModifiedUser { get; set; }
        public virtual TsaSchedule TsaSub { get; set; }
    }
}
