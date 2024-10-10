using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using TamsApi.Core;
using TamsApi.Models.Lookups;

namespace TamsApi.Models
{
    public partial class ChangeResolutionLog : ITrackModified, ITrackCreated
    {
        public static Dictionary<string, LogTypeId> RequestTypes = new Dictionary<string, LogTypeId>
        {
            {  "add", LogTypeId.Add },
            { "cancellation", LogTypeId.Cancellation },
            { "escalation", LogTypeId.Escalation },
            { "modification", LogTypeId.Modification }
        };
        public long ChangeResolutionLogId { get; set; }

        [StringLength(24)]
        public string TsaId { get; set; }

        [StringLength(24)]
        public string TsaSubId { get; set; }

        [StringLength(32)]
        public string ChangeId { get; set; }
        public int ChangeLogTypeId { get; set; }

        public string RequestType
        {
            get
            {
                if (Enum.IsDefined(typeof(LogTypeId), ChangeLogTypeId))
                {
                    return ((LogTypeId)ChangeLogTypeId).ToString();
                }

                return null;
            }
            set
            {
                var cleaned = value.IsPresent() ? value.ToLowerInvariant().Trim() : value;
                if (cleaned.IsPresent() && RequestTypes.ContainsKey(cleaned))
                {
                    ChangeLogTypeId = (int)RequestTypes[cleaned];
                }
            }
        }

        public DateTime? EffectiveDate { get; set; }

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

        [StringLength(128)]
        public string CostType { get; set; }

        [StringLength(1024)]
        public string AdditionalPricingComments { get; set; }

        [StringLength(256)]
        public string TaxDetermination { get; set; }

        [StringLength(24)]
        public string OptionExercisedFlag { get; set; }
        public int? ActiveOptionDurationExercised { get; set; }

        [Range(-9999999999999999.99, 9999999999999999.99)]
        public decimal? MonthlyCostForecast { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedUserId { get; set; }
        public User CreatedUser { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public long LastModifiedUserId { get; set; }
        public User LastModifiedUser { get; set; }
        public int FileRepositoryId { get; set; }

        [StringLength(640)]
        public string ProviderOwner { get; set; }
        [StringLength(8)]
        public string OptionFlag { get; set; }
        public int? MaxOptionDuration { get; set; }
        [Range(-9999999999999999.99, 9999999999999999.99)]
        public decimal? ExitCostAmount { get; set; }
        public DateTime? ActiveEndDate { get; set; }
        [StringLength(3)]
        public string Currency { get; set; }
        [StringLength(32)]
        public string SeparationOrStandalone { get; set; }
        public int? NoticeOfTermNum { get; set; }

        [Range(-9999999999999999.99, 9999999999999999.99)]
        public decimal? MonthlyPricing { get; set; }
        [StringLength(32)]
        public string MonthlyPricingText { get; set; }

        public virtual LogType ChangeLogType { get; set; }
        public virtual FileRepository FileRepository { get; set; }
        public virtual TsaSchedule TsaSub { get; set; }
    }
}
