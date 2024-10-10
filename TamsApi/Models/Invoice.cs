using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TamsApi.Models
{
    public partial class Invoice : ITrackCreated, ITsaChild
    {
        public int Id { get; set; }

        [StringLength(24)]
        public string TsaSubId { get; set; }

        [StringLength(24)]
        public string TsaId { get; set; }

        [StringLength(24)]
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }

        [StringLength(128)]
        public string InvoiceFrequency { get; set; }
        public DateTime? InvoicePeriodStartDate { get; set; }
        public DateTime? InvoicePeriodEndDate { get; set; }
        public DateTime? InvoiceDueDate { get; set; }
        public DateTime? ServicePeriod { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Markup { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedUserId { get; set; }
        public int FileRepositoryId { get; set; }

        [StringLength(3)]
        public string Currency { get; set; }

        [Range(-999999999999.999999, 999999999999.999999)]
        public decimal? UsdExchangeRate { get; set; }

        [Range(-9999999999999999.99, 9999999999999999.99)]
        public decimal? CostUsd { get; set; }
        [Range(-9999999999999999.99, 9999999999999999.99)]
        public decimal? MarkupUsd { get; set; }
        [Range(-9999999999999999.99, 9999999999999999.99)]
        public decimal? InvoiceAmountUsd { get; set; }

        public virtual User CreatedUser { get; set; }
        public virtual FileRepository FileRepository { get; set; }
        public virtual TsaSchedule TsaSub { get; set; }

        [StringLength(128)]
        public string UnitOfMeasure { get; set; }
        [Range(-9999999999999999.99, 9999999999999999.99)]
        public decimal? NumberOfUnits { get; set; }
        [StringLength(24)]
        public string ReferenceInvoiceNumber { get; set; }
    }
}
