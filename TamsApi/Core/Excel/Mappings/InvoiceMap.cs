using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TamsApi.Models;

namespace TamsApi.Core.Excel.Mappings
{
    public class InvoiceMap : ExcelMapProvider<Invoice>
    {
        public static ExcelMap Map { get; } = new ExcelMap {
            PrimaryKey = nameof(Invoice.TsaSubId),
            DbContextType = typeof(Invoice),
            Mappings = new List<ExcelColumnMapping> {
                CreateColumnMap(nameof(Invoice.TsaId), "TSA ID", nameof(String)),
                CreateColumnMap(nameof(Invoice.TsaSubId), "TSA Sub-ID", nameof(String)),
                CreateColumnMap(nameof(Invoice.InvoiceNumber), "Invoice Number", nameof(String)),
                CreateColumnMap(nameof(Invoice.InvoiceDate), "Invoice Date", "datetime?"),
                CreateColumnMap(nameof(Invoice.InvoiceFrequency), "Invoice Frequency", nameof(String)),
                CreateColumnMap(nameof(Invoice.InvoicePeriodStartDate), "Invoice Period Start Date", "datetime?"),
                CreateColumnMap(nameof(Invoice.InvoicePeriodEndDate), "Invoice Period End Date", "datetime?"),
                CreateColumnMap(nameof(Invoice.InvoiceDueDate), "Invoice Due Date", "datetime?"),
                CreateColumnMap(nameof(Invoice.ServicePeriod), "Service Period", "datetime?"),
                CreateColumnMap(nameof(Invoice.Cost), "Cost", "decimal?"),
                CreateColumnMap(nameof(Invoice.Markup), "Markup", "decimal?"),
                CreateColumnMap(nameof(Invoice.InvoiceAmount), "Invoice Amount", "decimal?"),
                CreateColumnMap(nameof(Invoice.Currency), "Currency", nameof(String)),
                CreateColumnMap(nameof(Invoice.UsdExchangeRate), "USD Exchange Rate", "decimal?"),
                CreateColumnMap(nameof(Invoice.CostUsd), "Cost USD", "decimal?"),
                CreateColumnMap(nameof(Invoice.MarkupUsd), "Markup USD", "decimal?"),
                CreateColumnMap(nameof(Invoice.InvoiceAmountUsd), "Invoice Amount USD", "decimal?"),
                CreateColumnMap(nameof(Invoice.UnitOfMeasure), "Unit of Measure", nameof(String)),
                CreateColumnMap(nameof(Invoice.NumberOfUnits), "Number of Units", "decimal?"),
                CreateColumnMap(nameof(Invoice.ReferenceInvoiceNumber), "Original Invoice Related to Credit Memo", nameof(String))
            }
        };

        public override ExcelMap ExcelMap => Map;
    }
}
