using System;
using System.Linq;
using System.Web.Mvc;
using Apex.Core.Validations;
using Apex.Shared.Extensions;

namespace Apex.Service.Extensions
{
    public static class ValidationErrorExtension
    {
        public static string JoinMessages(this ValidationErrorCollection errorCollection, string separator = "<br/>")
        {
            return errorCollection?.Any() ?? false ? string.Join(separator, errorCollection.Select(TranslateErrorCode).Distinct().ToArray()) :
                "عملیات با مشکل مواجه گردید.";
        }
        public static string JoinMessages(this Exception exception, string separator = "<br/>")
        {
            return TranslateErrorCode(exception);
        }

        private static string TranslateErrorCode(ValidationError error) => BaseTranslate(error.Name);

        private static string TranslateErrorCode(Exception exception) => BaseTranslate(exception.HResult.ToString());


        private static string BaseTranslate(string key)
        {
            switch (key)
            {
                case "1000":
                    return "این دسته بندی برای برخی از مطالب انتخاب شده است و امکان حذف آن وجود ندارد.";
                case "1001":
                    return "ابتدا زیرگروه های مربوط به این دسته بندی را حذف نمایید.";
                case "5000":
                    return "سفارش باید حداقل شامل یک آیتم باشد.";
                case "5001":
                    return "تعداد برخی از اقلام سفارش صفر میباشد.";
                case "6000":
                    return "رمز فعلی وارد شده اشتباه میباشد.";
                case "6002":
                    return "کد صنعتگر تکراری میباشد.";
                default:
                    return "عملیات با مشکل مواجه گردید.";

            }
        }
        public static string JoinMessages(this ModelStateDictionary dictionary, string separator = "<br/>")
        {
            var errors = dictionary.Values.Where(x => x.Errors.Any()).SelectMany(m => m.Errors.Select(x => x.ErrorMessage));
            return string.Join(separator, errors).IsNeu(BaseTranslate(""));
        }


        public static void ClearError(this ModelStateDictionary dictionary, Func<string, bool> func)
        {
            dictionary.Keys.Where(func).ToList().ForEach(x => dictionary[x].Errors.Clear());
        }
    }
}