using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using System.Web.Mvc;
using Apex.DAL.Abstracts;
using Apex.Shared.Extensions;
using Apex.Shared.Helpers;

namespace Apex.Service.Translations
{
    public static class StrHelper
    {
        public static void CreateJsLocals()
        {
            var repository = (ILanguageRepository)DependencyResolver.Current.GetService(typeof(ILanguageRepository));
            var langs = repository.Assets.ToList();


            var keys = typeof(Str).GetProperties().Where(x => x.PropertyType == typeof(string)).Select(x => x.Name).ToList();
            var path = HttpContext.Current.Server.MapPath("~/Locals");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            foreach (var lang in langs)
            {
                var c = keys.Select(x => $"{x}:\"{Str.ResourceManager.GetString(x, new CultureInfo(lang.Locale))}\"").Jn(",");
                // ReSharper disable once LocalizableElement
                File.WriteAllText($"{path}\\{lang.Locale}.js", $"const Str={{{c}}};");
            }

            // ReSharper disable once LocalizableElement
            File.WriteAllText(HttpContext.Current.Server.MapPath("~/Areas/Theme1/Scripts/_TS/src/types/IStr.d.ts"),
                $"interface IStr{{{keys.Select(x => $"{x}:string;").Jn("")}}}");
        }

        public static string Ft(this string str, params object[] items)
        {
            var s = str;
            if (s.Contains("{"))
            {
                for (var i = 0; i < items.Length; i++)
                    s = s.Replace($"{{{i}}}", items[0].ToString());
            }
            else
                s = $"{s} {items.Jn(" ")}";

            return s.Trim().Capitalize();
        }
    }
    public class ConventionalModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            try
            {
                var attr = attributes as Attribute[] ?? attributes.ToArray();

                if (!AppConst.Auth.IsDashboard(HttpContext.Current.Request.Url.Authority) && attr.Any())
                {
                    var display = (DisplayAttribute)attr.FirstOrDefault(x => x is DisplayAttribute);
                    if (display != null) display.ResourceType = typeof(Str);

                    var vs = attr.OfType<ValidationAttribute>();
                    foreach (var v in vs)
                    {
                        if (!v.ErrorMessage.IsNeu())
                        {
                            v.ErrorMessageResourceType = typeof(Str);
                            v.ErrorMessageResourceName = v.ErrorMessage;
                            v.ErrorMessage = null;
                        }
                    }
                }

                return base.CreateMetadata(attr, containerType, modelAccessor, modelType, propertyName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}