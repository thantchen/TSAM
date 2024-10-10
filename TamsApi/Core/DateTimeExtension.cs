using Newtonsoft.Json.Linq;using System;

namespace TamsApi.Core
{
    public static class DateTimeExtension
    {
        public static DateTime ToEasternTimeZone(this DateTime value)
        {
            TimeZoneInfo Eastern_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime dateTime_Eastern = TimeZoneInfo.ConvertTimeFromUtc(value, Eastern_Standard_Time);

            return dateTime_Eastern;
        }
    }
}
