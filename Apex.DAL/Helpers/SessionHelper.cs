using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using Apex.Core.Entities.LocaleE;
using Apex.DAL.Abstracts;
using Apex.Shared.Extensions;
using Apex.Shared.Helpers;

namespace Apex.DAL.Helpers
{
    public static class SessionHelper
    {
        public static HttpSessionState Session => HttpContext.Current.Session;
        private static ILanguageRepository LanguageRepository => (ILanguageRepository)DependencyResolver.Current.GetService(typeof(ILanguageRepository));

        public static T Get<T>(string typeName = "")
        {
            try
            {
                typeName = typeName.IsNeu() ? typeof(T).Name : typeName;
                switch (typeName)
                {
                    case AppConst.SessionKeys.Languages:
                        if (Session[typeName] == null)
                            Session[typeName] = LanguageRepository.Queryable().ToList();
                        break;
                }

                return (T)Session[typeName];
            }
            catch
            {
                return default;
            }
        }

        public static void Set<T>(object value = null)
        {
            Set(typeof(T).Name, value);
        }

        public static void Set(string typeName, object value = null)
        {
            try
            {
                Session[typeName] = value;
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public static List<Language> Languages => Get<List<Language>>(AppConst.SessionKeys.Languages);

    }
}