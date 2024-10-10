﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace TsaData.Models;

public partial class FileRepository
{
    public int FileRepositoryId { get; set; }

    public string FileName { get; set; }

    public byte[] FileStream { get; set; }

    public DateTime CreatedDate { get; set; }

    public long CreatedUserId { get; set; }

    public int FileTypeId { get; set; }

    public virtual ICollection<AddLogFile> AddLogFiles { get; set; } = new List<AddLogFile>();

    public virtual ICollection<ChangeLogFile> ChangeLogFiles { get; set; } = new List<ChangeLogFile>();

    public virtual ICollection<ChangeResolutionLog> ChangeResolutionLogs { get; set; } = new List<ChangeResolutionLog>();

    public virtual User CreatedUser { get; set; }

    public virtual ICollection<DisputeLogFile> DisputeLogFiles { get; set; } = new List<DisputeLogFile>();

    public virtual FileType FileType { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<SellSideNotificationChange> SellSideNotificationChanges { get; set; } = new List<SellSideNotificationChange>();

    public virtual ICollection<SellSideNotificationDispute> SellSideNotificationDisputes { get; set; } = new List<SellSideNotificationDispute>();

    public virtual ICollection<TsaSchedule> TsaSchedules { get; set; } = new List<TsaSchedule>();
}