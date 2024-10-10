using System.Collections.Generic;
using System.Linq;
using TamsApi.Core.Excel;

namespace TamsApi.Core
{
    public static class CollectionExtensions
    {
        public static bool Blank<T>(this T[] values) => values == null || values.Length == 0;
        public static bool Blank<T>(this IEnumerable<T> values) => values == null || values.Count() == 0;

        public static List<string> Flatten(this List<FailedRow> failures)
        {
            return failures.SelectMany(f => f.Errors.Select(e => $"{e} (Row {f.RowIndex})")).ToList();
        }
    }
}
