using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Apex.Shared.Extensions;
using Apex.Web.Models;

namespace Apex.Web.Extensions
{
    public static class ExtensionMethods
    {

        public static BaseUrl GetBaseUrl(this ActionExecutingContext context)
        {
            var controller = context.RouteData.Values["controller"].ToString().Capitalize();
            var action = context.RouteData.Values["action"].ToString().Capitalize();
            return new BaseUrl(new UrlHelper(context.RequestContext), action, controller);
        }

        /// <summary>
        /// Return name of file without format as long. format 'yyyyMMddHHmmss'
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public static long GetDateNameL(this FileInfo fileInfo)
        {
            return long.Parse(Regex.Replace(fileInfo.Name, "\\..+", ""));
        }

        /// <summary>
        /// Return name of file without format as string.
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public static string GetDateNameS(this FileInfo fileInfo)
        {
            return Regex.Replace(fileInfo.Name, "\\..+", "");
        }

        /// <summary>
        /// Return DateTime reperesented by file name with format 'yyyyMMddHHmmss'
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime GetDateNameD(this FileInfo fileInfo, string format = "yyyyMMddHHmmss")
        {
            var m = DateTime.ParseExact(fileInfo.GetDateNameS(), format, CultureInfo.InvariantCulture);
            return m;
        }
    }
}