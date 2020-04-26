using System.Text.RegularExpressions;
using Apex.Shared.Extensions;

namespace Apex.Shared.Helpers
{
    public static class AppConst
    {
        public static class Ui
        {
            public const string JsVoid = "javascript:void(0)";
        }
        public static class HeaderKey
        {
            public const string Culture = "culture";
        }
        public static class TempDataKey
        {
            public const string ExportXls = "Td_ExportXls";
        }
        public static class SessionKeys
        {
            public const string OperatorPermission = "Ss_OperatorPermission";
            public const string Languages = "Ss_Languages";
            public const string CurrentCulture = "Ss_CurrentCulture";
        }
        public static class CookieKeys
        {
            public const string Language = "Ck_Lang";
        }

        public static class Auth
        {
            public const string Admin = "admin";
            public const string DevPass = "@Ahmad100";
            public static bool IsDashboard(string authority)
            {
                return Regex.IsMatch(authority, "^admin\\.", RegexOptions.IgnoreCase);
            }
        }
        public static class Lang
        {
            public const string Fa = "fa";
            public const string Farsi = "فارسی";

            public const string En = "en";
            public const string English = "English";
        }

        public class Folder
        {
            public const string MasterInfo = "MasterInfo";
            public const string Product = "Product";
            public const string Slider = "Slider";
        }
        public class RegexPattern
        {
            public const string Culture = "^[a-zA-Z]{2}$";
            public const string Id = "^\\d+\\b";
            public const string Mobile = "^09\\d{9}$";
        }
        public class MessagePattern
        {
            public static string SmsPass(string name, string pass, string expire)
            {
                return $"{name.IsNeu("کاربر").Trim()} عزیز\nرمز عبور: {pass}\nانقضاء: {expire}";
            }
        }
    }
}