using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace TamsApi.Core
{
    public static class StringExtensions
    {
        public static bool IsPresent(this string value)
        {
            return !value.IsBlank();
        }

        public static bool IsBlank(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Extract list of property from JSON string data and convert to List<T>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this string value, string propertyName)
        {
            return JObject.Parse(value)[propertyName].Select(x => x.ToObject<T>()).ToList();
        }
    }
}
