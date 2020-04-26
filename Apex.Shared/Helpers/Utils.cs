using System;
using System.Text.RegularExpressions;
using log4net;

namespace Apex.Shared.Helpers
{
    public static class Utils
    {
        public static ILog Logger { get; set; } = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Tuple<decimal, decimal> Round10000(decimal inp)
        {
            var rm = inp % 10000;
            return Tuple.Create(inp - rm, rm);
        }

        public static decimal RoundAmount(this decimal inp)
        {
            var rm = inp % 10000;
            return rm > 5000 ? 10000 - rm : -rm;
        }

        public static string GenerateGuid(int length = 6)
        {
            var guid = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
            return guid.Substring(0, Math.Min(guid.Length, length));
        }
    }
}
