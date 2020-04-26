using System.Collections.Generic;
using Apex.Shared.Extensions;

namespace Apex.Web.Helpers
{
    public static class ImageHelper
    {
        //public static string Scale(string url, int width, int height)
        //{
        //    if (url.IsNeu()) return url;
        //    return $"{url}.ashx?{(width > 0 ? $"w={width}&" : "")}{(width > 0 ? $"h={height}&" : "")}";
        //}

        /// <summary>
        /// Not found image.
        /// </summary>
        public static string Nf1 = "/Areas/Theme1/Images/nf.png";

        public static string Scale(this string url, int width, int height, string bgcolor = null)
        {

            if (url.IsNeu()) return url;
            var u = $"{url}.ashx?";
            var ps = new List<string>();

            if (width > 0) ps.Add($"w={width}");
            if (height > 0) ps.Add($"h={height}");
            if (!bgcolor.IsNeu()) ps.Add($"bgcolor={bgcolor?.Replace("#", "")}");

            return $"{u}{ps.Jn("&")}";
        }


    }
}