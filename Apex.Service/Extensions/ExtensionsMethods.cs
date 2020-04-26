using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Apex.Service.Extensions
{
    public static class ExtensionMethods
    {
        public static TAttribute Attr<TAttribute>(this Type type, string propertyName) where TAttribute : Attribute
        {
            var property = type.GetProperty(propertyName);
            return property?.GetCustomAttribute<TAttribute>();
        }

        public static string AttrName<T>(Expression<Func<T, string>> expression)
        {
            var property = typeof(T).GetProperty((expression.Body as MemberExpression)?.Member.Name ?? throw new InvalidOperationException());
            return property?.GetCustomAttribute<DisplayAttribute>()?.Name;
        }

        public static bool IsNullable(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }



        /// <summary>
        /// Return name of file without format as long. format 'yyyyMMddHHmmssffff'
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public static long NameAsLong(this FileInfo fileInfo)
        {
            return long.Parse(Regex.Replace(fileInfo.Name, "\\..+", ""));
        }

        /// <summary>
        /// Return name of file without format as string.
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public static string NameAsString(this FileInfo fileInfo)
        {
            return Regex.Replace(fileInfo.Name, "\\..+", "");
        }

        /// <summary>
        /// Return DateTime reperesented by file name with format 'yyyyMMddHHmmssffff'
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime NameAsDate(this FileInfo fileInfo, string format = "yyyyMMddHHmmssffff")
        {
            var m = DateTime.ParseExact(fileInfo.NameAsString(), format, CultureInfo.InvariantCulture);
            return m;
        }
    }
}