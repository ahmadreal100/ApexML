using System.Text.RegularExpressions;
using Apex.DAL.Helpers;
using Apex.Service.Enums;
using Apex.Shared.Extensions;
using Apex.Shared.Helpers;

namespace Apex.Service.Extensions
{
    public static class LinkCreator
    {
        public static string Lang => CultureHelper.Get();
        public static string Create(LinkType type, long id, string title)
        {
            var name = Regex.Replace(title.IsNeu(""), "\\W", "-");
            name = title.IsNeu() ? "" : $"-{name}";
            switch (type)
            {
                case LinkType.Category:
                    return $"/{Lang}/search?categoryId={id}";
                case LinkType.Product:
                    return $"/{Lang}/product/{id}{name}";
                default:
                    return AppConst.Ui.JsVoid;
            }
        }
    }
}