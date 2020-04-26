using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Apex.Web.Models;

namespace Apex.Web.Infrastructure
{
    public class AjaxHandlerAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.RequestContext.HttpContext.Request;
            if (!IsException(filterContext) && request.IsAjaxRequest() && !request.IsAuthenticated)
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                filterContext.Result = new JsonResult
                {
                    Data = new AjaxResult("مدت زمان احراز هویت شما سپری شده است. لطفا مجددا وارد شوید."),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
                base.OnAuthorization(filterContext);
        }

        private bool IsException(ControllerContext filterContext)
        {
            return new[] { "Account-CheckUserName", "Account-Login" }.Any(x =>
               string.Equals(x, $"{filterContext.RouteData.Values["controller"]}-{filterContext.RouteData.Values["action"]}",
                   StringComparison.OrdinalIgnoreCase));
        }
    }
}