using Microsoft.AspNetCore.Routing.Template;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace TamsApi.Core
{
    public static class DictionaryExtensions
    {
        public static bool IsNotEmpty<TKey, TValue>(this IDictionary<TKey, TValue> items)
        {
            return !items.IsEmpty();
        }

        public static bool IsEmpty<TKey, TValue>(this IDictionary<TKey, TValue> items)
        {
            return items == null || items.Count == 0;
        }

        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> target, IDictionary<TKey, TValue> source)
        {
            var newDictionary = new Dictionary<TKey, TValue>(target.Count + source.Count);
            foreach(var kvp in target)
            {
                newDictionary.Add(kvp.Key, kvp.Value);
            }

            foreach (var kvp in source)
            {
                newDictionary.Add(kvp.Key, kvp.Value);
            }
            return newDictionary;
        }
    }
}
