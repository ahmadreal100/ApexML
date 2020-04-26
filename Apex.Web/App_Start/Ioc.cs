using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Apex.Core;
using Apex.DAL;
using Apex.Service;
using Apex.Service.Extensions;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using AutofacDependencyResolver = Autofac.Integration.Mvc.AutofacDependencyResolver;

namespace Apex.Web
{
    public static class Ioc
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(Ioc).Assembly);

            // OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinderProvider();

            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            // OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();


            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);

            // OPTIONAL: Register the Autofac model binder provider.
            builder.RegisterWebApiModelBinderProvider();

            builder.RegisterModule(new IocCore());
            builder.RegisterModule(new IocService());
            builder.RegisterModule(new IocDal());

            //            builder.Register(
            //                    c => HttpContext.Current != null ?
            //                        new HttpContextWrapper(HttpContext.Current) :
            //                        c.Resolve<System.Net.Http.HttpRequestMessage>().Properties["MS_HttpContext"])
            //                .As<HttpContextBase>()
            //                .InstancePerRequest();
            //            builder.RegisterHubs(Assembly.GetExecutingAssembly());

            builder.Register(m =>
            {
                var info = new RequestInfo();
                var user = HttpContext.Current.User ?? Thread.CurrentPrincipal;

                info.UserName=user.Identity.Name;
                info.UserId = user.Identity.GetUserIdLong();
                info.UserType = user.Identity.GetUserType();
                //info.LangId = RequestHelper.GetLanguage().Id;

                return info;
            }).InstancePerRequest();


            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            //GlobalHost.DependencyResolver = new AutofacDependencyResolver1(container);
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}