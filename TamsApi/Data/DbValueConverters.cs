using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TamsApi.Data
{
    public static class DbValueConverters
    {
        public static ValueConverter<IDictionary<string, object>, string> JsonDictionaryConverter { get; } =
            new ValueConverter<IDictionary<string, object>, string>(
                v => SerializeDictionary(v),
                (v) => DeserializeDictionary(v));

        public static string SerializeDictionary(IDictionary<string, object> v)
        {
            var cells = v ?? new Dictionary<string, object>();
            return JsonConvert.SerializeObject(cells);
        }

        public static IDictionary<string, object> DeserializeDictionary(string v)
        {
            var data = v ?? "{}";
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(data);
        }

        public static ValueConverter<DateTime, DateTime> UtcDateTimeConverter
        {
            get => new ValueConverter<DateTime, DateTime>(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        }

        public static ValueConverter<DateTime?, DateTime?> UtcNullableDateTimeConverter
        {
            get => new ValueConverter<DateTime?, DateTime?>(
                v => v.HasValue ? v.Value.ToUniversalTime() : v,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);
        }
    }
}
