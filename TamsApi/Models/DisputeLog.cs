using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TamsApi.Models
{
    public partial class DisputeLog : ITrackCreated, ITrackModified, ITrackRevisions
    {
        public DisputeLog()
        {
            DisputeLogFile = new HashSet<DisputeLogFile>();
        }

        public int DisputeLogId { get; set; }
        public string TsaSubId { get; set; }
        public string TsaId { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public int DisputeTypeId { get; set; }
        public string DisputeId { get; set; }
        public string InvoiceNumber { get; set; }
        public long RequestedByUserId { get; set; }
        public User RequestedByUser { get; set; }
        public long SubmittedByUserId { get; set; }
        public User SubmittedByUser { get; set; }
        public int DisputeDiscrepancyId { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        [MaxLength]
        public string Comments { get; set; }
        public decimal? AgreedCost { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedUserId { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public long LastModifiedUserId { get; set; }
        public User LastModifiedUser { get; set; }
        public int Revision { get; set; }
        public DateTime? ServicePeriod { get; set; }

        [StringLength(3)]
        public string Currency { get; set; }

        public virtual User CreatedUser { get; set; }
        public virtual DisputeDiscrepancy DisputeDiscrepancy { get; set; }
        public virtual DisputeType DisputeType { get; set; }
        public virtual TsaSchedule TsaSub { get; set; }
        public virtual ICollection<DisputeLogFile> DisputeLogFile { get; set; }
    }
}
