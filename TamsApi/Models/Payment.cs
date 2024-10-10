using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TamsApi.Models
{
    public partial class Payment : ITrackCreated, ITsaChild
    {
        public int PaymentId { get; set; }

        [StringLength(24)]
        public string TsaSubId { get; set; }

        [StringLength(24)]
        public string TsaId { get; set; }

        [StringLength(24)]
        public string InvoiceNumber { get; set; }

        [StringLength(128)]
        public string PaymentType { get; set; }

        [Range(-9999999999999999.99, 9999999999999999.99)]
        public decimal? PaymentAmount { get; set; }
        public DateTime? PaymentDate { get; set; }

        [StringLength(256)]
        public string TransactionId { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedUserId { get; set; }
        public int FileRepositoryId { get; set; }

        [StringLength(3)]
        public string Currency { get; set; }

        [Range(-999999999999.999999, 999999999999.999999)]
        public decimal? ExchangeRate { get; set; }

        [StringLength(256)]
        public string BankAccountFrom { get; set; }

        [StringLength(256)]
        public string BankAccountTo { get; set; }

        [Range(-9999999999999999.99, 9999999999999999.99)]
        public decimal? UsdConversion { get; set; }

        public virtual User CreatedUser { get; set; }
        public virtual FileRepository FileRepository { get; set; }
        public virtual TsaSchedule TsaSub { get; set; }
    }
}
