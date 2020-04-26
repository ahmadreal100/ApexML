using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using Apex.Shared.Extensions;

namespace Apex.Shared.Helpers
{
    public static class DateHelper
    {
        public static string TimeStamp => Now.ToString("yyyyMMddHHmmssffff");

        public static string ToPersian(this DateTime date, string pattern = "yyyy/MM/dd")
        {
            try
            {
                return date.ToString(pattern ?? "yyyy/MM/dd", new CultureInfo("fa-IR"));
            }
            catch
            {
                return "";
            }
        }
        public static string ToCalendar(this DateTime? date, string pattern = "yyyy/MM/dd")
        {
            return date?.ToCalendar(pattern) ?? "";
        }
        public static string ToCalendar(this DateTime date, string pattern = "yyyy/MM/dd")
        {
            return date.ToString(pattern.IsNeu("yyyy/MM/dd"), new CultureInfo(Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName));
        }
        public static string ToPersian(this DateTime? date, string pattern = "yyyy/MM/dd")
        {
            return date?.ToPersian(pattern) ?? "";
        }

        public static DateTime? ToGregorian(this string date)
        {
            try
            {
                if (date.IsNeu()) return null;
                if (DateTime.TryParse(date, new CultureInfo("fa-IR"), DateTimeStyles.None, out var pDate))
                    return DateTime.Parse(pDate.ToString(new CultureInfo("en-US")));
            }
            catch
            {
                //
            }
            return null;
        }

        public static DateTime Now => DateTime.Now;


        public static long Stamp()
        {
            return Now.Ticks;
        }
    }
}
