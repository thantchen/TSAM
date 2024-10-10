using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TamsApi.Data
{
    public static class DbValueComparers
    {
        public static ValueComparer<IDictionary<string, object>> KeyOnlyDictionaryComparer { get; } =
            new ValueComparer<IDictionary<string, object>>(
                (d1, d2) => d1.Keys.SequenceEqual(d2.Keys),
                d => d.GetHashCode(),
                d => d);
    }
}
