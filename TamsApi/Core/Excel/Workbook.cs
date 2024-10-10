using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Globalization;
using System.IO;

namespace TamsApi.Core.Excel
{
    public static class Workbook
    {
        // NPOI uses thread local with statics to hold onto culture
        // and locale info for controlling how it parses culture sensitive things
        // within Excel files. The bug rears its head in Aspnet Core after a single
        // import is done on a TSA schedule. The fix is to always set the culture and
        // and locale prior to parsing an Excel document. This is ok because we want 
        // to control how NPOI parses the file regardless of the current culture and 
        // locale information - plus we don't want the bug re-appearing.
        public static CultureInfo DefaultCulture = new CultureInfo("en-US");
        public static TimeZoneInfo DefaultTzInfo = TimeZoneInfo.Utc;

        public static IWorkbook OpenRead(FileInfo fileInfo)
        {
            // Configure NPOI utils.
            ConfigureNpoi();
            return new XSSFWorkbook(fileInfo);
        }

        public static IWorkbook OpenRead(Stream stream)
        {
            ConfigureNpoi();
            return new XSSFWorkbook(stream);
        }

        [Obsolete("Replace with SetUserTimeZone(DefaultTzInfo) when next version of NPOI is released.")]
        public static void ConfigureNpoi()
        {
            NPOI.Util.LocaleUtil.SetUserLocale(DefaultCulture);
            NPOI.Util.LocaleUtil.SetUserTimeZone(TimeZoneInfo.Utc);
        }

        [Obsolete("Will need to fix this when NPOI releases their fix to LocaleUtil")]
        // We should be able to delete this class with the next release of NPOI.
        internal class UtcTimeZone : TimeZone
        {
            public override string DaylightName => DefaultTzInfo.DaylightName;

            public override string StandardName => DefaultTzInfo.StandardName;

            public override DaylightTime GetDaylightChanges(int year)
            {
                // No daylight savings support per MS documentation.
                // https://docs.microsoft.com/en-us/dotnet/api/system.timezone.getdaylightchanges?view=netcore-3.1
                return new DaylightTime(DateTime.MinValue, DateTime.MinValue, new TimeSpan(0));
            }

            public override TimeSpan GetUtcOffset(DateTime time)
            {
                return DefaultTzInfo.GetUtcOffset(time);
            }
        }
    }
}
