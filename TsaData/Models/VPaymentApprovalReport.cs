﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace TsaData.Models;

public partial class VPaymentApprovalReport
{
    public string TsaId { get; set; }

    public string TsaDescription { get; set; }

    public string Currency { get; set; }

    public string OriginalInvoiceNumber { get; set; }

    public DateOnly? ServicePeriod { get; set; }

    public decimal? OriginalInvoiceAmountUsd { get; set; }

    public string CreditMemoNumber { get; set; }

    public decimal CreditMemoAmountUsd { get; set; }

    public decimal? DisputedAmountUsd { get; set; }

    public decimal? ApprovedProposedReleaseWoTax { get; set; }

    public decimal? TotalInvoicedAmountForServicePeriod { get; set; }

    public decimal? TsaBaseline { get; set; }

    public decimal? DifferenceWB { get; set; }
}