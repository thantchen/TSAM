using Microsoft.AspNetCore.Components.Forms;
using System;
using TamsApi.Models;

namespace TamsApi.Core.Excel.Mappings
{
    public interface IExcelMapProvider<TModel> where TModel : class, new()
    {
        ExcelMap ExcelMap { get; }
    }

    public abstract class ExcelMapProvider<TModel> : IExcelMapProvider<TModel> where TModel : class, new()
    {
        public static ExcelColumnMapping CreateColumnMap(string dbColumn, string excelHeader, string castType,
            bool isMergeCell = false)
        {
            return new ExcelColumnMapping
            {
                DbColumn = dbColumn,
                ExcelHeader = excelHeader,
                CastType = castType,
                IsMergedCell = isMergeCell
            };
        }

        public abstract ExcelMap ExcelMap { get; }
    }
}
