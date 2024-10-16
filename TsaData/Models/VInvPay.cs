﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace TsaData.Models;

public partial class VInvPay
{
    public string TsaSubId { get; set; }

    public string TsaId { get; set; }

    public string InvoiceNumber { get; set; }

    public string AnkuraInvoice { get; set; }

    public DateOnly? InvoiceDate { get; set; }

    public DateOnly? InvoicePeriodStartDate { get; set; }

    public DateOnly? InvoicePeriodEndDate { get; set; }

    public DateOnly? ServicePeriod { get; set; }

    public string Currency { get; set; }

    public decimal? InvoiceAmount { get; set; }

    public decimal? InvoiceAmountUsd { get; set; }

    public string SeparationOrStandalone { get; set; }

    public string Function { get; set; }

    public string SubFunction { get; set; }

    public string ServiceName { get; set; }

    public string TsaVsRtsa { get; set; }

    public string PrimaryOwner { get; set; }

    public string ReceiverOwner { get; set; }

    public string DisputeCategory { get; set; }

    public decimal? AgreedCost { get; set; }

    public string Comments { get; set; }

    public decimal? PaymentAmount { get; set; }

    public decimal? PaymentUsdConversion { get; set; }

    public DateOnly? PaymentDate { get; set; }

    public string PaymentType { get; set; }

    public string TransactionId { get; set; }
}