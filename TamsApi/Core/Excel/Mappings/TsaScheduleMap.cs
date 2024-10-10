using System;
using System.Collections.Generic;
using TamsApi.Models;

namespace TamsApi.Core.Excel.Mappings
{
    public class TsaScheduleMap : ExcelMapProvider<TsaSchedule>
    {
        public static ExcelMap Map { get; } = new ExcelMap
            {
                DbContextType = typeof(TsaSchedule),
                PrimaryKey = nameof(TsaSchedule.TsaId),
                Mappings = new List<ExcelColumnMapping> {
                    CreateColumnMap(nameof(TsaSchedule.TsaId), "TSA ID", nameof(String), isMergeCell: true),
                    CreateColumnMap(nameof(TsaSchedule.Function), "Function", nameof(String), isMergeCell: true),
                    CreateColumnMap(nameof(TsaSchedule.SubFunction), "Sub-Function", nameof(String), isMergeCell: true),
                    CreateColumnMap(nameof(TsaSchedule.ServiceName), "Service Name", nameof(String), isMergeCell: true),
                    CreateColumnMap(nameof(TsaSchedule.ProviderOwner), "Provider Owner", nameof(String), isMergeCell: true),
                    CreateColumnMap(nameof(TsaSchedule.ReceiverOwner), "Receiver Owner", nameof(String), isMergeCell: true),
                    CreateColumnMap(nameof(TsaSchedule.ServiceExceptions), "Service Exceptions", nameof(String), isMergeCell: true),
                    CreateColumnMap(nameof(TsaSchedule.ServiceLocation), "Service Location", nameof(String), isMergeCell: true),
                    CreateColumnMap(nameof(TsaSchedule.EstimatedDurationText), "Estimated Duration (Months)", nameof(String), isMergeCell: true),
                    CreateColumnMap(nameof(TsaSchedule.Assumptions), "Assumptions", nameof(String), isMergeCell: true),
                    CreateColumnMap(nameof(TsaSchedule.ExceptionsToMonthlyInvoice), "Exceptions to monthly invoicing", nameof(String), isMergeCell: true),
                    CreateColumnMap(nameof(TsaSchedule.ExceptionsTo30DayPayment), "Exceptions to 30 day payment terms", nameof(String), isMergeCell: true),
                    CreateColumnMap(nameof(TsaSchedule.NoticeOfTerminationText), "Notice of termination", nameof(String), isMergeCell: true),
                    CreateColumnMap(nameof(TsaSchedule.ExceptionsReOwnershipIp), "Exceptions regarding ownership of Intellectual Property", nameof(String), isMergeCell: true),
                    CreateColumnMap(nameof(TsaSchedule.TsaExitApproach), "TSA Exit Approach", nameof(String), isMergeCell: true),
                    CreateColumnMap(nameof(TsaSchedule.KnownTsaExitCosts), "Known TSA Exit Costs", nameof(String), isMergeCell: true),
                    CreateColumnMap(nameof(TsaSchedule.KnownDependencies), "Known Interdependencies", nameof(String), isMergeCell: true),
                    CreateColumnMap(nameof(TsaSchedule.DetailedServiceDescription), "Detailed Service Description", nameof(String), isMergeCell: true),
                    // Start of Sub fields that are not in merged cells.
                    CreateColumnMap(nameof(TsaSchedule.TsaSubId), "TSA Sub-ID", nameof(String)),
                    CreateColumnMap(nameof(TsaSchedule.TsaVsRtsa), "TSA vs RTSA", nameof(String)),
                    CreateColumnMap(nameof(TsaSchedule.UnitDescription), "Unit Description", nameof(String)),
                    CreateColumnMap(nameof(TsaSchedule.NumOfUnits), "# of Units", nameof(String)),
                    CreateColumnMap(nameof(TsaSchedule.PricePerUnit), "Price / Unit", nameof(String)),
                    CreateColumnMap(nameof(TsaSchedule.MonthlyPricing), "Monthly Pricing", "currency?"),
                    CreateColumnMap(nameof(TsaSchedule.Usd), "USD", "currency?"),
                    CreateColumnMap(nameof(TsaSchedule.Inr), "INR", "decimal?"),
                    CreateColumnMap(nameof(TsaSchedule.Crc), "CRC", "decimal?"),
                    CreateColumnMap(nameof(TsaSchedule.Mxn), "MXN", "decimal?"),
                    CreateColumnMap(nameof(TsaSchedule.Php), "PHP", "decimal?"),
                    CreateColumnMap(nameof(TsaSchedule.Bgn), "BGN", "decimal?"),
                    CreateColumnMap(nameof(TsaSchedule.Brl), "BRL", "decimal?"),
                    CreateColumnMap(nameof(TsaSchedule.Cad), "CAD", "decimal?"),
                    CreateColumnMap(nameof(TsaSchedule.Eur), "EUR", "decimal?"),
                    CreateColumnMap(nameof(TsaSchedule.Gbp), "GBP", "decimal?"),
                    CreateColumnMap(nameof(TsaSchedule.Dkk), "DKK", "decimal?"),
                    CreateColumnMap(nameof(TsaSchedule.Pln), "PLN", "decimal?"),
                    CreateColumnMap(nameof(TsaSchedule.Cny), "CNY", "decimal?"),
                    CreateColumnMap(nameof(TsaSchedule.Huf), "HUF", "decimal?"),
                    CreateColumnMap(nameof(TsaSchedule.Sgd), "SGD", "decimal?"),
                    CreateColumnMap(nameof(TsaSchedule.Aud), "AUD", "decimal?"),
                    CreateColumnMap(nameof(TsaSchedule.Czk), "CZK", "decimal?"),
                    CreateColumnMap(nameof(TsaSchedule.CostType), "Cost Type", nameof(String)),
                    CreateColumnMap(nameof(TsaSchedule.AdditionalPricingComments), "Additional Pricing Comments", nameof(String)),
                    CreateColumnMap(nameof(TsaSchedule.TaxDetermination), "Tax Determination", nameof(String)),
                    CreateColumnMap(nameof(TsaSchedule.EstimatedDuration), "Estimated Duration (Number of Months)", nameof(Int32)),
                    CreateColumnMap(nameof(TsaSchedule.NoticeOfTermNum), "Notice of termination (Number)", nameof(Int32)),
                    CreateColumnMap(nameof(TsaSchedule.OptionFlag), "Option (Y/N)", nameof(String)),
                    CreateColumnMap(nameof(TsaSchedule.MaxOptionDuration), "Maximum Option Duration (Months)", "int?"),
                    CreateColumnMap(nameof(TsaSchedule.OptionExercisedFlag), "Option Exercised (Y/N/TBD)", nameof(String)),
                    CreateColumnMap(nameof(TsaSchedule.ActiveOptionDurationExercised), "Active Option Duration Exercised (Months)", "int?"),
                    CreateColumnMap(nameof(TsaSchedule.ExitCostAmount), "Exit Cost Amount", "currency?"),
                    CreateColumnMap(nameof(TsaSchedule.ChangeLogType), "Change Log Type", nameof(String)),
                    CreateColumnMap(nameof(TsaSchedule.MonthlyCostForecast), "Monthly Cost Forecast", "currency?"),
                    CreateColumnMap(nameof(TsaSchedule.StartDate), "Start Date", "datetime?"),
                    CreateColumnMap(nameof(TsaSchedule.OriginalEndDate), "Original End Date", "datetime?"),
                    CreateColumnMap(nameof(TsaSchedule.ActiveEndDate), "Active End Date", "datetime?"),
                    CreateColumnMap(nameof(TsaSchedule.SeparationOrStandalone), "Separation Or Standalone", nameof(String)),
                    CreateColumnMap(nameof(TsaSchedule.PrimaryOwner), "Primary Owner", nameof(String)),
                    CreateColumnMap(nameof(TsaSchedule.MonthlyPricingText), "Monthly Pricing (Text)", nameof(String)),
                }
        };

        public override ExcelMap ExcelMap => Map;
    }
}
