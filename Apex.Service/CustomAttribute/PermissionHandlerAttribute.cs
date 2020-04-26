using System.Web.Mvc;
using Accounting.Service.Abstracts;

namespace Accounting.Service.CustomAttribute
{
    public class PermissionHandlerAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userService = (IUserService)DependencyResolver.Current.GetService(typeof(IUserService));
            var info = userService.RequestInfo;
            //if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            //{
            //    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            //    filterContext.Result = new JsonResult
            //    {
            //        Data = new AjaxResult
            //        {
            //            Status = false,
            //            Message = "مدت زمان احراز هویت شما سپری شده است. لطفا مجددا وارد شوید."
            //        },
            //        JsonRequestBehavior = JsonRequestBehavior.AllowGet
            //    };
            //}
        }

        //private bool Valid()
        //{
        //    filterContext.RouteData.Values["controller"].ToString().Capitalize();
        //}
    }
}