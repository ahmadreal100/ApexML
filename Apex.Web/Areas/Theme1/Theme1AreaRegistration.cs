using System.Web.Mvc;
using Apex.Shared.Extensions;
using Apex.Shared.Helpers;
using Apex.Web.Infrastructure;

namespace Apex.Web.Areas.Theme1
{
    public class Theme1AreaRegistration : AreaRegistration
    {
        public override string AreaName => "Theme1";

        private static string[] NameSpaces => new[]
        {
            "Apex.Web.Areas.Theme1.Controllers"
        };

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Theme1_Localized",
                "{culture}/{controller}/{action}/{id}",
                new { area = "Theme1", controller = "Home", action = "Index", id = UrlParameter.Optional },
                new { area = new ThemeConstraint(), culture = AppConst.RegexPattern.Culture},
                NameSpaces
            ).DataTokens["area"]=AreaName;
            context.MapRoute(
                "Theme1_Default",
                "{controller}/{action}/{id}",
                new { area = "Theme1", controller = "Home", action = "Index", id = UrlParameter.Optional },
                new { area = new ThemeConstraint()},
                NameSpaces
            ).DataTokens["area"]=AreaName;
        }
    }
}