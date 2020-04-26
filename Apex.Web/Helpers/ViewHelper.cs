using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apex.Web.Helpers
{
    public static class ViewHelper
    {

        public static HtmlString BuildShortcutTitle(Dictionary<string, string> keys)
        {
            var str = string.Empty;
            keys.ToList().ForEach(x => str += $"[{x.Key}] : {x.Value} <br/>");
            return new HtmlString(str);
        }
    }
}