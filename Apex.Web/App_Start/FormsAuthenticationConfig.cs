//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web;
//using Apex.Web.App_Start;
//using Apex.Web.Infrastructure;
//using Microsoft.Web.Infrastructure.DynamicModuleHelper;
//
//[assembly: PreApplicationStartMethod(typeof(FormsAuthenticationConfig), "Register")]
//
//namespace Apex.Web.App_Start
//{
//    public static class FormsAuthenticationConfig
//    {
//        public static void Register()
//        {
//            DynamicModuleUtility.RegisterModule(typeof(SuppressFormsAuthenticationRedirectModule));
//        }
//    }
//}
