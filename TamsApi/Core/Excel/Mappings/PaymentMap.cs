using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TamsApi.Models;

namespace TamsApi.Core.Excel.Mappings
{
    public class PaymentMap : ExcelMapProvider<Payment>
    {
        public static ExcelMap Map { get; } = new ExcelMap {
            PrimaryKey = nameof(Payment.TsaSubId),
            DbContextType = typeof(Payment),
            Mappings = new List<ExcelColumnMapping> {
                CreateColumnMap(nameof(Payment.TsaId), "TSA ID", nameof(String)),
                CreateColumnMap(nameof(Payment.TsaSubId), "TSA Sub-ID", nameof(String)),
                CreateColumnMap(nameof(Payment.InvoiceNumber), "Invoice Number", nameof(String)),
                CreateColumnMap(nameof(Payment.PaymentAmount), "Payment Amount", "currency?"),
                CreateColumnMap(nameof(Payment.Currency), "Currency", nameof(String)),
                CreateColumnMap(nameof(Payment.PaymentDate), "Payment Date", "datetime?"),
                CreateColumnMap(nameof(Payment.PaymentType), "Payment Type", nameof(String)),
                CreateColumnMap(nameof(Payment.TransactionId), "Transaction ID or Check Number", nameof(String)),
                CreateColumnMap(nameof(Payment.BankAccountFrom), "From Bank Account Number (Milano)", nameof(String)),
                CreateColumnMap(nameof(Payment.BankAccountTo), "To Bank Account Number (DXC)", nameof(String)),
                CreateColumnMap(nameof(Payment.ExchangeRate), "USD Exchange Rate", "decimal?"),
                CreateColumnMap(nameof(Payment.UsdConversion), "USD Conversion", "currency?"),
            }
        };

        public override ExcelMap ExcelMap => Map;
    }
}
