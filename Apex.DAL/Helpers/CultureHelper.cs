using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using Apex.Core.Entities.LocaleE;
using Apex.Shared.Extensions;

namespace Apex.DAL.Helpers
{
    public static class CultureHelper
    {
        public static HttpContext Context => HttpContext.Current;
        public static HttpSessionState Session => HttpContext.Current.Session;
        public static HttpResponse Response => HttpContext.Current.Response;

        public static string Get()
        {
            return Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower();
        }
        public static void Set(string locale)
        {
            if (!locale.Contains("/"))
                Set(locale);
            else
            {
                HasLanguage(locale, out var lc);
                Set(lc);
            }

            void Set(string l)
            {
                //Thread.CurrentThread.CurrentCulture = culture;
                //We want to use culture only in translation and ui theme.
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(l);
            }
        }

        public static bool HasLanguage(string absoluteUri)
        {
            return HasLanguage(absoluteUri, out _);
        }
        public static bool HasLanguage(string absoluteUri, out string lng)
        {
            var lcs = SessionHelper.Languages.Select(x => x.Locale.ToLower());

            if (!Regex.IsMatch(absoluteUri, "^http", RegexOptions.IgnoreCase))
                return Has(absoluteUri, out lng);

            var u = new Uri(absoluteUri);
            return Has(u.AbsolutePath, out lng);

            bool Has(string p, out string l)
            {
                var m = Regex.Match(p, $@"^/(?:{lcs.Jn("|")})(?=(?:\?|/|$))").Value;
                l = m.IsNeu() ? Default.Locale : m.Replace("/", "");
                return !m.IsNeu();
            }
        }

        public static string SetLanguage(string absoluteUri, string locale)
        {
            if (!Regex.IsMatch(absoluteUri, "^http", RegexOptions.IgnoreCase))
                return Replace(absoluteUri);

            var u = new Uri(absoluteUri);
            var path = Replace(u.AbsolutePath);
            return $"{u.Scheme}://{u.Authority}{path}{u.Query}";

            string Replace(string p) => Regex.Replace(p, @"^/(\w{2}(?=$|/|\?))?", $"/{locale}/", RegexOptions.IgnoreCase).Replace("//","/");
        }

        public static string SetLanguage(string absoluteUri, Language language)
        {
            return SetLanguage(absoluteUri, language.Locale);
        }

        public static Language CurrentLanguage()
        {
            var locale = Get();
            return SessionHelper.Languages.OrderBy(x => x.Locale.ToLower() == locale.ToLower()).Last();
        }
        public static Language Default
        {
            get
            {
                return SessionHelper.Languages.OrderBy(x => x.IsDefault).Last();
            }
        }

        public static bool IsRtl => CurrentLanguage().Rtl;
    }
}