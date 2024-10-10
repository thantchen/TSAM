﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace TsaData.Models;

public partial class DisputeLog
{
    public int DisputeLogId { get; set; }

    public string TsaSubId { get; set; }

    public string TsaId { get; set; }

    public DateOnly? SubmissionDate { get; set; }

    public int DisputeTypeId { get; set; }

    public string DisputeId { get; set; }

    public string InvoiceNumber { get; set; }

    public long RequestedByUserId { get; set; }

    public long SubmittedByUserId { get; set; }

    public int DisputeDiscrepancyId { get; set; }

    public string Comments { get; set; }

    public decimal? AgreedCost { get; set; }

    public DateTime CreatedDate { get; set; }

    public long CreatedUserId { get; set; }

    public DateTime LastModifiedDate { get; set; }

    public long LastModifiedUserId { get; set; }

    public int Revision { get; set; }

    public string Currency { get; set; }

    public DateOnly? ServicePeriod { get; set; }

    public virtual User CreatedUser { get; set; }

    public virtual DisputeDiscrepancy DisputeDiscrepancy { get; set; }

    public virtual ICollection<DisputeLogFile> DisputeLogFiles { get; set; } = new List<DisputeLogFile>();

    public virtual DisputeType DisputeType { get; set; }

    public virtual User LastModifiedUser { get; set; }

    public virtual User RequestedByUser { get; set; }

    public virtual User SubmittedByUser { get; set; }

    public virtual TsaSchedule TsaSub { get; set; }
}