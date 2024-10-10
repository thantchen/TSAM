using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TamsApi.Models
{
    public partial class TsaSchedule
    {
        public TsaSchedule()
        {
            AddLog = new HashSet<AddLog>();
            ChangeLog = new HashSet<ChangeLog>();
            ChangeResolutionLog = new HashSet<ChangeResolutionLog>();
            DisputeLog = new HashSet<DisputeLog>();
            Invoice = new HashSet<Invoice>();
            Payment = new HashSet<Payment>();
            SellSideNotificationChange = new HashSet<SellSideNotificationChange>();
            SellSideNotificationDispute = new HashSet<SellSideNotificationDispute>();
        }

        [Required]
        [StringLength(24)]
        public string TsaSubId { get; set; }

        [StringLength(24)]
        public string TsaId { get; set; }

        [StringLength(128)]
        public string Function { get; set; }

        [StringLength(128)]
        public string SubFunction { get; set; }

        [StringLength(512)]
        public string ServiceName { get; set; }

        public string DetailedServiceDescription { get; set; }

        [StringLength(16)]
        public string TsaVsRtsa { get; set; }

        [StringLength(640)]
        public string PrimaryOwner { get; set; }

        [StringLength(640)]
        public string ReceiverOwner { get; set; }

        [StringLength(1024)]
        public string ServiceExceptions { get; set; }

        [StringLength(512)]
        public string ServiceLocation { get; set; }

        [StringLength(64)]
        public string EstimatedDurationText { get; set; }

        public string Assumptions { get; set; }

        [StringLength(512)]
        public string ExceptionsToMonthlyInvoice { get; set; }

        [StringLength(256)]
        public string ExceptionsTo30DayPayment { get; set; }

        [StringLength(128)]
        public string NoticeOfTerminationText { get; set; }

        [StringLength(256)]
        public string ExceptionsReOwnershipIp { get; set; }

        [StringLength(512)]
        public string TsaExitApproach { get; set; }

        [StringLength(512)]
        public string KnownTsaExitCosts { get; set; }

        [StringLength(1024)]
        public string KnownDependencies { get; set; }

        [StringLength(1024)]
        public string UnitDescription { get; set; }

        [StringLength(128)]
        public string NumOfUnits { get; set; }

        [StringLength(128)]
        public string PricePerUnit { get; set; }

        [Range(-9999999999999999.99, 9999999999999999.99)]
        public decimal? MonthlyPricing { get; set; }

        [Range(-9999999999999999.99, 9999999999999999.99)]
        public decimal? Usd { get; set; }

        [Range(-9999999999999999.99, 9999999999999999.99)]
        public decimal? Inr { get; set; }

        [Range(-9999999999999999.99, 9999999999999999.99)]
        public decimal? Crc { get; set; }

        [Range(-9999999999999999.99, 9999999999999999.99)]
        public decimal? Mxn { get; set; }

        [Range(-9999999999999999.99, 9999999999999999.99)]
        public decimal? Php { get; set; }

        [Range(-9999999999999999.99, 9999999999999999.99)]
        public decimal? Bgn { get; set; }

        [Range(-9999999999999999.99, 9999999999999999.99)]
        public decimal? Brl { get; set; }

        [Range(-9999999999999999.99, 9999999999999999.99)]
        public decimal? Cad { get; set; }

        [Range(-9999999999999999.99, 9999999999999999.99)]
        public decimal? Eur { get; set; }

        [Range(-9999999999999999.99, 9999999999999999.99)]
        public decimal? Gbp { get; set; }

        [Range(-9999999999999999.99, 9999999999999999.99)]
        public decimal? Dkk { get; set; }

        [Range(-9999999999999999.99, 9999999999999999.99)]
        public decimal? Pln { get; set; }

        [Range(-9999999999999999.99, 9999999999999999.99)]
        public decimal? Cny { get; set; }

        [Range(-9999999999999999.99, 9999999999999999.99)]
        public decimal? Huf { get; set; }

        [Range(-9999999999999999.99, 9999999999999999.99)]
        public decimal? Sgd { get; set; }

        [Range(-9999999999999999.99, 9999999999999999.99)]
        public decimal? Aud { get; set; }

        [Range(-9999999999999999.99, 9999999999999999.99)]
        public decimal? Czk { get; set; }

        [StringLength(3)]
        public string Currency { get; set; }

        [StringLength(128)]
        public string CostType { get; set; }

        [StringLength(1024)]
        public string AdditionalPricingComments { get; set; }

        [StringLength(256)]
        public string TaxDetermination { get; set; }
        public int? EstimatedDuration { get; set; }
        public int? NoticeOfTermNum { get; set; }
        [StringLength(8)]
        public string OptionFlag { get; set; }
        public int? MaxOptionDuration { get; set; }

        [StringLength(24)]
        public string OptionExercisedFlag { get; set; }
        public int? ActiveOptionDurationExercised { get; set; }

        [Range(-9999999999999999.99, 9999999999999999.99)]
        public decimal? ExitCostAmount { get; set; }

        [StringLength(24)]
        public string ChangeLogType { get; set; }

        [Range(-9999999999999999.99, 9999999999999999.99)]
        public decimal? MonthlyCostForecast { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? OriginalEndDate { get; set; }
        public DateTime? ActiveEndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedUserId { get; set; }
        public User CreatedUser { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public long LastModifiedUserId { get; set; }
        public User LastModifiedUser { get; set; }
        public int FileRepositoryId { get; set; }
        public int Revision { get; set; }

        [StringLength(32)]
        public string SeparationOrStandalone { get; set; }
        [StringLength(640)]
        public string ProviderOwner { get; set; }
        [StringLength(32)]
        public string MonthlyPricingText { get; set; }

        public virtual FileRepository FileRepository { get; set; }
        public virtual ICollection<AddLog> AddLog { get; set; }
        public virtual ICollection<ChangeLog> ChangeLog { get; set; }
        public virtual ICollection<ChangeResolutionLog> ChangeResolutionLog { get; set; }
        public virtual ICollection<DisputeLog> DisputeLog { get; set; }
        public virtual ICollection<Invoice> Invoice { get; set; }
        public virtual ICollection<Payment> Payment { get; set; }
        public virtual ICollection<SellSideNotificationChange> SellSideNotificationChange { get; set; }
        public virtual ICollection<SellSideNotificationDispute> SellSideNotificationDispute { get; set; }
    }
}
