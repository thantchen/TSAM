using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using NPOI.SS.UserModel;

namespace TamsApi.Core.Excel
{
    public static class CellUtils
    {
        public static List<Regex> NoValueFlags = new List<Regex> { 
            new Regex(@"\btbd\b", RegexOptions.IgnoreCase),
            new Regex("n/a", RegexOptions.IgnoreCase),
            new Regex("fixed", RegexOptions.IgnoreCase),
            new Regex("varies", RegexOptions.IgnoreCase),
			new Regex("actual", RegexOptions.IgnoreCase),
            new Regex("-", RegexOptions.IgnoreCase)};
        public static IFormatProvider UsFormatProvider { get; } = new CultureInfo("en-US").NumberFormat;
        public static NumberStyles UsCurrencyStyles { get; } = 
            NumberStyles.AllowCurrencySymbol | NumberStyles.AllowThousands |
            NumberStyles.AllowParentheses | NumberStyles.AllowDecimalPoint;

        public static DateTimeStyles UsDateStyles { get; } =
            DateTimeStyles.NoCurrentDateDefault | DateTimeStyles.AssumeUniversal |
            DateTimeStyles.AdjustToUniversal;

        public static readonly Dictionary<string, Func<IEvaluatedCell, object>> TypeMap = new Dictionary<string, Func<IEvaluatedCell, object>>
        {
            ["string"] = cell => ParseString(cell),
            ["datetime"] = cell => ParseDateTime(cell),
            ["datetime?"] = cell => ParseNullableDateTime(cell),
            ["decimal"] = cell => ParseDecimal(cell),
            ["decimal?"] = cell => ParseNullableDecimal(cell),
            ["int32"] = cell => ParseInt(cell),
            ["int"] = cell => ParseInt(cell),
            ["int?"] = cell => ParseNullableInt(cell),
            ["currency"] = cell => ParseCurrency(cell),
            ["currency?"] = cell => ParseNullableCurrency(cell)
        };

        public static string ParseString(IEvaluatedCell cell)
        {
            if (cell.IsBlank()) return null;

            if (cell.IsString())
            {
                var value = cell.StringCellValue.Trim();
                return value.IsPresent() ? value : null;
            }

            switch(cell.CellType)
            {
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                case CellType.Numeric:
                case CellType.Formula:
                    return cell.NumericCellValue.ToString();
            }

            ThrowInvalidCast("string", cell.CellType.ToString(), cell);
            return null; /* will never get here because of error */
        }

        public static DateTime? ParseNullableDateTime(IEvaluatedCell cell)
        {
            if (cell.IsBlank() || cell.HasNoValueFlag()) return null;

            if (cell.IsString())
            {
                var value = cell.StringCellValue.Trim();
                if (value.IsBlank()) return null;

                if(DateTime.TryParse(value, UsFormatProvider, UsDateStyles, out var parseDate))
                {
                    if (parseDate.Date != default(DateTime).Date) // We do not support time only parsing for now.
                    {
                        return parseDate;
                    }
                }

                ThrowInvalidCast("datetime", value, cell);
            }

            if (cell.CellType == CellType.Numeric)
            {
                return DateTime.FromOADate(cell.NumericCellValue);
            }

            return cell.DateCellValue;
        }

        public static DateTime ParseDateTime(IEvaluatedCell cell)
        {
            return ParseNullableDateTime(cell) ?? default;
        }

        public static decimal? ParseNullableDecimal(IEvaluatedCell cell)
        {
            if (cell.IsBlank() || cell.HasNoValueFlag()) return null;

            if (cell.IsString())
            {
                var value = cell.StringCellValue.Trim();
                if (value.IsBlank()) return null;

                if (decimal.TryParse(value, out var parsedDecimal))
                {
                    return parsedDecimal;
                }

                ThrowInvalidCast("decimal", value, cell);
            }

            return Convert.ToDecimal(cell.NumericCellValue);
        }

        public static decimal? ParseDecimal(IEvaluatedCell cell)
        {
            return ParseNullableDecimal(cell) ?? default;
        }

        public static int? ParseNullableInt(IEvaluatedCell cell)
        {
            if (cell.IsBlank() || cell.HasNoValueFlag()) return null;

            if (cell.IsString())
            {
                var value = cell.StringCellValue.Trim();
                if (value.IsBlank()) return null;

                if (int.TryParse(value, UsCurrencyStyles, UsFormatProvider, out var parsedInt))
                {
                    return parsedInt;
                }

                ThrowInvalidCast("integer", value, cell);
            }

            return Convert.ToInt32(cell.NumericCellValue);
        }

        public static int ParseInt(IEvaluatedCell cell)
        {
            return ParseNullableInt(cell) ?? default; 
        }

        public static decimal ParseCurrency(IEvaluatedCell cell)
        {
            return ParseNullableCurrency(cell) ?? default;
        }

        public static decimal? ParseNullableCurrency(IEvaluatedCell cell)
        {
            if (cell.IsBlank() || cell.HasNoValueFlag()) return null;

            if (cell.IsString())
            {
                var value = cell.StringCellValue.Trim();
                if (value.IsBlank()) return null;

                if (decimal.TryParse(value, UsCurrencyStyles, UsFormatProvider, out decimal parsedValue))
                {
                    return parsedValue;
                }

                ThrowInvalidCast("currency", value, cell);
            }

            return ParseDecimal(cell);
        }

        public static void ThrowInvalidCast(string type, string value, IEvaluatedCell cell)
        {
            int number = cell.ColumnIndex+1; // cell starts with zero-based index
            bool isCaps = true;
            Char c = (Char)((isCaps ? 65 : 97) + (number - 1));

            throw new InvalidCastException($"Cannot convert cell {number} ({c}) to {type}: {value}");
        }

        public static bool IsBlank(this IEvaluatedCell cell)
        {
            return cell == null || cell.CellType == CellType.Blank || cell.RawCell.IsBlank();
        }

        public static bool IsBlank(this ICell cell)
        {
            return cell == null || cell.CellType == CellType.Blank;
        }

        public static bool IsPresent(this IEvaluatedCell cell)
        {
            return !cell.IsBlank();
        }

        public static bool IsPresent(this ICell cell)
        {
            return !cell.IsBlank();
        }

        public static bool IsString(this IEvaluatedCell cell)
        {
            return cell.CellType == CellType.String;
        }

        public static bool IsString(this ICell cell)
        {
            return cell.CellType == CellType.String;
        }

        public static bool HasNoValueFlag(this IEvaluatedCell cell)
        {
            if (!cell.IsString()) return false;

            var cellValue = cell.StringCellValue?.Trim();
            return cellValue.IsPresent() && NoValueFlags.Any(re => re.IsMatch(cellValue));
        }
    }
}
