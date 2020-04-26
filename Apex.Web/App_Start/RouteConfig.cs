using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Apex.Shared.Extensions;
using Apex.Web.Infrastructure;
using WebGrease.Css.Extensions;

namespace Apex.Web
{
    public class RouteConfig
    {
        private static string[] NameSpaces => new[]
        {
            "Apex.Web.Controllers",
            "Apex.Web.Controllers.Account",
            "Apex.Web.Controllers.Base",
            "Apex.Web.Controllers.Content",
            "Apex.Web.Controllers.Shop"
        };

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            foreach (var route in RouteTable.Routes.OfType<IReadOnlyCollection<RouteBase>>())
            {
                route.OfType<Route>().ForEach(r =>
                {
                    r.Url = $"{{culture}}{(r.Url.IsNeu() ? "" : "/")}{r.Url}";
                    r.Defaults.Add("area", "Theme1");
                    r.Constraints.Add("culture", "^[a-zA-Z]{2}$");
                    r.Constraints.Add("area", new ThemeConstraint());
                    r.DataTokens.Add("area", "Theme1");
                });
            }

            AreaRegistration.RegisterAllAreas();


            routes.MapRoute(
                "Main_Localized",
                "{culture}/{controller}/{action}/{id}",
                new { area = "admin", controller = "Home", action = "Index", id = UrlParameter.Optional },
                new { culture = "^[a-zA-Z]{2}$", area = new ThemeConstraint() },
                NameSpaces
            );
            routes.MapRoute(
                "Main_Default",
                "{controller}/{action}/{id}",
                new { area = "admin", controller = "Home", action = "Index", id = UrlParameter.Optional },
                new { area = new ThemeConstraint() },
                NameSpaces
            );
        }
    }
}