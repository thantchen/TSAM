using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TamsApi.Models;

namespace TamsApi.Core.Excel.Mappings
{
    public class SellSideDisputeMap : ExcelMapProvider<SellSideNotificationDispute>
    {
        public static ExcelMap Map { get; } = new ExcelMap
        {
            PrimaryKey = nameof(SellSideNotificationDispute.TsaSubId),
            DbContextType = typeof(SellSideNotificationDispute),
            Mappings = new List<ExcelColumnMapping> {
                CreateColumnMap(nameof(SellSideNotificationDispute.TsaId), "tsa_id", nameof(String)),
                CreateColumnMap(nameof(SellSideNotificationDispute.TsaSubId), "tsa_sub_id", nameof(String)),
                CreateColumnMap(nameof(SellSideNotificationDispute.PrimaryOwner), "primary_owner", nameof(String)),
                CreateColumnMap(nameof(SellSideNotificationDispute.ServiceName), "service_name", nameof(String)),
                CreateColumnMap(nameof(SellSideNotificationDispute.Type), "type", nameof(String)),
                CreateColumnMap(nameof(SellSideNotificationDispute.InvoicePeriodStartDate), "invoice_period_start_date", "datetime?"),
                CreateColumnMap(nameof(SellSideNotificationDispute.InvoicePeriodEndDate), "invoice_period_end_date", "datetime?"),
                CreateColumnMap(nameof(SellSideNotificationDispute.InvoiceNumber), "invoice_number", nameof(String)),
                CreateColumnMap(nameof(SellSideNotificationDispute.InvoiceAmount), "invoice_amount", "currency?"),
                CreateColumnMap(nameof(SellSideNotificationDispute.InvoiceCurrency), "invoice_currency", nameof(String)),
                CreateColumnMap(nameof(SellSideNotificationDispute.ActionItem), "action_item", nameof(String))
            }
        };

        public override ExcelMap ExcelMap => Map;
    }
}
