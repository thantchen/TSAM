using System;
using System.Collections.Generic;
using TamsApi.Models;

namespace TamsApi.Core.Excel.Mappings
{
    public class SellSideChangeMap : ExcelMapProvider<SellSideNotificationChange>
    {
        public static ExcelMap Map { get; } = new ExcelMap
        {
            PrimaryKey = nameof(SellSideNotificationChange.TsaSubId),
            DbContextType = typeof(SellSideNotificationChange),
            Mappings = new List<ExcelColumnMapping> {
                CreateColumnMap(nameof(SellSideNotificationChange.TsaId), "tsa_id", nameof(String)),
                CreateColumnMap(nameof(SellSideNotificationChange.TsaSubId), "tsa_sub_id", nameof(String)),
                CreateColumnMap(nameof(SellSideNotificationChange.PrimaryOwner), "primary_owner", nameof(String)),
                CreateColumnMap(nameof(SellSideNotificationChange.ServiceName), "service_name", nameof(String)),
                CreateColumnMap(nameof(SellSideNotificationChange.Status), "status", nameof(String)),
                CreateColumnMap(nameof(SellSideNotificationChange.ActionItem), "action_item", nameof(String))
            }
        };

        public override ExcelMap ExcelMap => Map;
    }
}
