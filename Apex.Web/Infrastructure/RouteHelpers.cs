using System.Web;
using System.Web.Routing;
using Apex.Shared.Extensions;
using Apex.Shared.Helpers;

namespace Apex.Web.Infrastructure
{
    public class ThemeConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var area = values["area"].ToString();
            if (AppConst.Auth.IsDashboard(httpContext.Request.Url?.Authority))
                return area.Eq("admin");

            return area.Eq("theme1");
        }
    }
}