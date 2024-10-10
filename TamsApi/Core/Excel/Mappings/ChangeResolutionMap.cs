using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TamsApi.Models;

namespace TamsApi.Core.Excel.Mappings
{
    public class ChangeResolutionMap : ExcelMapProvider<ChangeResolutionLog>
    {
        public static ExcelMap Map { get; } = new ExcelMap
        {
            DbContextType = typeof(ChangeResolutionLog),
            PrimaryKey = nameof(ChangeResolutionLog.TsaSubId),
            Mappings = new List<ExcelColumnMapping> {
                    CreateColumnMap(nameof(ChangeResolutionLog.TsaId), "TSA ID", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.TsaSubId), "Sub-TSA ID", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.ChangeId), "Change ID", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.RequestType), "Change Request Type", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.EffectiveDate), "Effective Date", "datetime?"),
                    CreateColumnMap(nameof(ChangeResolutionLog.Function), "Function", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.SubFunction), "Sub-Function", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.ServiceName), "Service Name", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.DetailedServiceDescription), "Detailed Service Description", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.TsaVsRtsa), "TSA vs RTSA", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.ProviderOwner), "Provider Owner", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.ReceiverOwner), "Receiver Owner", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.ServiceExceptions), "Service Exceptions", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.ServiceLocation), "Service Location", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.EstimatedDurationText), "Estimated Duration (Months)", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.Assumptions), "Assumptions", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.ExceptionsToMonthlyInvoice), "Exceptions to monthly invoicing", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.ExceptionsTo30DayPayment), "Exceptions to 30 day payment terms", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.NoticeOfTerminationText), "Notice of termination", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.ExceptionsReOwnershipIp), "Exceptions regarding ownership if Intellectual Property", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.TsaExitApproach), "TSA Exit Approach", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.KnownTsaExitCosts), "Known TSA Exit Costs", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.KnownDependencies), "Known Interdependencies", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.UnitDescription), "Unit Description", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.NumOfUnits), "# of Units", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.PricePerUnit), "Price / Unit*", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.CostType), "Cost Type", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.AdditionalPricingComments), "Additional Pricing Comments", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.TaxDetermination), "Tax Determination", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.OptionExercisedFlag), "Option Exercised (Y/N/TBD)", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.ActiveOptionDurationExercised), "Active Option Duration Exercised (Months)", "int?"),
                    CreateColumnMap(nameof(ChangeResolutionLog.MonthlyCostForecast), "Monthly Cost Forecast", "currency?"),
                    CreateColumnMap(nameof(ChangeResolutionLog.PrimaryOwner), "Primary Owner", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.OptionFlag), "Option (Y/N)", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.MaxOptionDuration), "Maximum Option Duration (Months)", "int?"),
                    CreateColumnMap(nameof(ChangeResolutionLog.ExitCostAmount), "Exit Cost Amount", "currency?"),
                    CreateColumnMap(nameof(ChangeResolutionLog.ActiveEndDate), "Active End Date", "datetime?"),
                    CreateColumnMap(nameof(ChangeResolutionLog.Currency), "Currency", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.SeparationOrStandalone), "Separation Or Standalone", nameof(String)),
                    CreateColumnMap(nameof(ChangeResolutionLog.NoticeOfTermNum), "Notice of termination (Number)", nameof(Int32)),
                    CreateColumnMap(nameof(ChangeResolutionLog.MonthlyPricing), "Monthly Pricing", "currency?"),
                    CreateColumnMap(nameof(ChangeResolutionLog.MonthlyPricingText), "Monthly Pricing (Text)", nameof(String)),
                }
        };
        public override ExcelMap ExcelMap => Map;
    }
}
