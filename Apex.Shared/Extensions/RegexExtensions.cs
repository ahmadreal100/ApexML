using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Apex.Shared.Extensions
{
    public static class RegexExtensions
    {
        public static List<string> ToValueList(this MatchCollection collection, bool toLower = false)
        {
            return toLower ? collection.Cast<Match>().Select(m => m.Value.ToLower()).ToList() :
                collection.Cast<Match>().Select(m => m.Value).ToList();
        }
    }
}