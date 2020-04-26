using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Apex.DAL.Helpers;
using Apex.Service.Abstracts;
using Apex.Service.Translations;
using Apex.Web.Models.Binders;

namespace Apex.Web
{

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Ioc.Register();
            //IocHub.Register();
            var ac = (IUserService)DependencyResolver.Current.GetService(typeof(IUserService));

            Seed.Run(ac, Server.MapPath("locations.json"), Server.MapPath("langs.json"));
            MapperConfig.Init();

            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());

            ModelBinders.Binders.Add(typeof(double), new DoubleModelBinder());
            ModelBinders.Binders.Add(typeof(double?), new DoubleModelBinder());

            ModelBinders.Binders.Add(typeof(long), new LongModelBinder());
            ModelBinders.Binders.Add(typeof(long?), new LongModelBinder());

            ModelBinders.Binders.Add(typeof(int), new IntModelBinder());
            ModelBinders.Binders.Add(typeof(int?), new IntModelBinder());

            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeModelBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new DateTimeModelBinder());

            StrHelper.CreateJsLocals();
            ModelMetadataProviders.Current = new ConventionalModelMetadataProvider();
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            var url = Request.Url;
            if (Regex.IsMatch(url.AbsoluteUri, "\\.ashx")) return;

            if (!CultureHelper.HasLanguage(url.AbsolutePath))
            {
                var lng = CultureHelper.Default;
                var link = CultureHelper.SetLanguage(url.AbsoluteUri, lng);
                Response.Redirect(link, true);
            }

            CultureHelper.Set(url.AbsoluteUri);
        }
    }
}
