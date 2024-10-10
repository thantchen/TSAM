using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TamsApi.Data;

namespace TamsApi.Models
{
    public class VPaymentApprovalReport
    {
        public VPaymentApprovalReport()
        {
        }

        [Key]
        [Column("tsa_id")]
        public string TsaId { get; set; }
        [Column("tsa_description")]
        public string TsaDescription { get; set; }
        [Column("currency")]
        public string Currency { get; set; }
        [Column("original_invoice_number")]
        public string OriginalInvoiceNumber { get; set; }
        [Column("service_period")]
        public DateTime? ServicePeriod { get; set; }
        [Column("original_invoice_amount_usd", TypeName = "decimal(18,4)")]
        public decimal? OriginalInvoiceAmountUsd { get; set; }
        [Column("credit_memo_number")]
        public string CreditMemoNumber { get; set; }
        [Column("credit_memo_amount_usd", TypeName = "decimal(18,4)")]
        public decimal? CreditMemoAmountUsd { get; set; }
        [Column("disputed_amount_usd", TypeName = "decimal(18,4)")]
        public decimal? DisputedAmountUsd { get; set; }
        [Column("approved_proposed_release_wo_tax", TypeName = "decimal(18,4)")]
        public decimal? ApprovedProposedReleaseWOTax { get; set; }
        [Column("total_invoiced_amount_for_service_period", TypeName = "decimal(18,4)")]
        public decimal? TotalInvoiceAmountForServicePeriod { get; set; }
        [Column("tsa_baseline", TypeName = "decimal(18,4)")]
        public decimal? TsaBaseline { get; set; }
        [Column("difference_w_b", TypeName = "decimal(18,4)")]
        public decimal? DifferenceWB { get; set; }
    }

}
