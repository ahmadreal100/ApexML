using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Apex.Shared.Extensions
{
    public static class ExtensionMethods
    {
        public static string GetNameFromDir(this string str) => Regex.Match(str, @"(?<=[\\/])[^\\\/.]+\.[^\\\/.]+$").Value;
        public static string GetExtentionFromDir(this string str) => Regex.Match(str, @"[^\\\/.]+$").Value;

        public static string ToHashedGuid(this string str)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.Default.GetBytes(str));
                return new Guid(hash).ToString();
            }
        }
        public static string Capitalize(this string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Any())
                return str.First().ToString().ToUpper() + str.Substring(1).ToLower();
            return "";
        }
        public static string RemoveFirst(this string str, string find, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            var i = str.IsNeu("").IndexOf(find, comparison);
            if (i == -1)
                return str;
            return str.Remove(i, find.Length);
        }
        public static bool IsInteger(this string str)
        {
            return str.IsInteger(out int _);
        }
        public static bool IsInteger(this string str, out int outInt)
        {
            return int.TryParse(str, out outInt);
        }
        public static int IsInteger(this string str, int instead)
        {
            return int.TryParse(str, out int outI) ? outI : instead;
        }
        public static bool IsLong(this string str)
        {
            return str.IsLong(out long _);
        }
        public static bool IsLong(this string str, out long outLong)
        {
            return long.TryParse(str, out outLong);
        }
        public static long IsLong(this string str, long instead)
        {
            return long.TryParse(str, out long outI) ? outI : instead;
        }

        public static bool IsDouble(this string str)
        {
            return str.IsDouble(out double _);
        }
        public static bool IsDouble(this string str, out double outInt)
        {
            return double.TryParse(str, out outInt);
        }
        public static double IsDouble(this string str, double instead)
        {
            return double.TryParse(str, out double outD) ? outD : instead;
        }
        public static bool IsNeu(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }
        public static string IsNeu(this string str, string instead)
        {
            return string.IsNullOrWhiteSpace(str) ? instead : str;
        }
        public static string Ellipses(this string str, int length, string ellipses = "...")
        {
            if (str == null || str.Length < length)
                return str;

            return $"{new string(str.Take(length).ToArray())}{ellipses}";
        }
        public static long GetId(this string str)
        {
            return long.Parse(Regex.Match(str.IsNeu(""), "^\\d+\\b").Value.IsNeu("0"));
        }
        public static bool Eq(this string str, string str2)
        {
            return (str ?? "").ToLower() == (str2 ?? "").ToLower();
        }

        //-----------------Thousand---------------
        #region Thousand
        public static string Thousand(this string str, int decimalPlace = 2, string decimalFormat = "(#,0.##)")
        {
            return decimal.TryParse(str, out decimal outD) ? outD.ToString(BuildThousand(decimalPlace, decimalFormat)) : str;
        }
        public static string Thousand(this decimal num, int decimalPlace = 2, string decimalFormat = "(#,0.##)")
        {
            return num.ToString(BuildThousand(decimalPlace, decimalFormat));
        }
        public static string Thousand(this double num, int decimalPlace = 2, string decimalFormat = "(#,0.##)")
        {
            return num.ToString(BuildThousand(decimalPlace, decimalFormat));
        }
        public static string Thousand(this long num, int decimalPlace = 2, string decimalFormat = "(#,0.##)")
        {
            return num.ToString(BuildThousand(decimalPlace, decimalFormat));
        }
        public static string Thousand(this int num, int decimalPlace = 2, string decimalFormat = "(#,0.##)")
        {
            return num.ToString(BuildThousand(decimalPlace, decimalFormat));
        }

        private static string BuildThousand(int decimalPlace, string decimalFormat)
        {
            return $"#,0.{new string('#', decimalPlace)};{decimalFormat}";
        }

        #endregion
        //-----------------Thousand---------------


        public static List<MethodInfo> ControllerActions(string controllerName, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
        {
            return Type.GetType(controllerName)?.GetMethods(flags).ToList();
        }

        public static List<MethodInfo> ControllerActions<T>(BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
        {
            return typeof(T).GetMethods(flags).ToList();
        }

        public static List<KeyValuePair<string, string>> ToListKeyValuePair(this NameValueCollection collection)
        {
            var pairs = new List<KeyValuePair<string, string>>();
            foreach (string key in collection)
                pairs.Add(new KeyValuePair<string, string>(key, collection[key]));
            return pairs;
        }

        /// <summary>
        /// Return Name property of DisplayAttribute.
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string Name(this Enum enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString()).ToList()
                .First()
                .GetCustomAttribute<DisplayAttribute>().Name;
        }
        public static int ToInt(this Enum enumValue)
        {
            return (int)Enum.Parse(enumValue.GetType(), enumValue.ToString());
        }


        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString()).ToList()
                .First()
                .GetCustomAttribute<TAttribute>();
        }

        public static string[] GetMemeberNames(this Type type)
        {
            var ss = type.GetMembers().ToList();
            var sss = ss.Select(x => x.GetCustomAttribute<DisplayAttribute>().Name).ToArray();
            return sss;
        }

        public static string Description(this Enum value)
        {
            return !(value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() is DescriptionAttribute attribute) ? value.ToString() : attribute.Description;
        }

        public static string Message(this Enum enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString()).ToList()
                .First()
                .GetCustomAttribute<DisplayAttribute>().Description;
        }

        public static Dictionary<string, object> ToDictionary(this object dynObj)
        {
            var dictionary = new Dictionary<string, object>();
            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(dynObj))
            {
                var obj = propertyDescriptor.GetValue(dynObj);
                dictionary.Add(propertyDescriptor.Name, obj);
            }
            return dictionary;
        }
        public static string Jn<T>(this IEnumerable<T> list, string separator)
        {
            return string.Join(separator, list);
        }
    }
    public static class EnumHelper<T>
    {
        public static string[] GetMemeberNames()
        {
            var names = typeof(T).GetFields().Where(x => !x.IsSpecialName)
                .Select(x => x.GetCustomAttribute<DisplayAttribute>()?.Name).ToArray();
            return names;
        }
        public static Dictionary<int, string> ToDictionary(bool displayName)
        {
            if (displayName)
            {
                var dic = new Dictionary<int, string>();
                var names = GetMemeberNames();
                for (var i = 0; i < names.Length; i++)
                    dic.Add(i, names[i]);
                return dic;
            }
            return Enum.GetValues(typeof(T))
                 .Cast<T>()
                 .ToDictionary(t => int.Parse(t.ToString()), t => t.ToString());
        }
    }
    public static class SelectExtensions
    {
        public static SelectList SetSelectedValue(this SelectList selectList, object value)
        {
            return new SelectList(selectList.Items, selectList.DataValueField,
                selectList.DataTextField, selectList.DataGroupField, value,
                selectList.DisabledValues, selectList.DisabledGroups);
        }
        public static void SetSelectedValue(this List<SelectListItem> items, object value)
        {
            foreach (var item in items)
                item.Selected = item.Value == value.ToString();
        }
    }
}