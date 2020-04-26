using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Apex.Service.Abstracts;
using Apex.Service.ViewModels.Account;
using Apex.Shared.Extensions;
using Apex.Shared.Helpers;
using Apex.Web.Extensions;
using Apex.Web.Helpers;
using Apex.Web.Models;
using Apex.Web.Models.ViewData;

namespace Apex.Web.Infrastructure
{
    public class PermissionHandlerAttribute : ActionFilterAttribute
    {
        private readonly IUserService _userService;

        public PermissionHandlerAttribute()
        {
            _userService = (IUserService)DependencyResolver.Current.GetService(typeof(IUserService));
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var requestInfo = _userService.RequestInfo;
            if (!requestInfo.IsOperator || AttributeHelper.Has<IgnorePermissionAttribute>(filterContext))
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            var baseUrl = filterContext.GetBaseUrl().Url.ToLower();

            var urlHelper = new UrlHelper(filterContext.RequestContext);

            //Get all url that limited as permission.
            var allMenus = OptPermission.LayoutMenu(urlHelper);

            //Detect if request url was limited by permission or not.
            var menu = allMenus.FirstNested(x => x.HasUrl(baseUrl));
            var passed = true;
            if (menu != null)
            {
                //Get operator permission.
                var menuPermissions = GetOperatorPermission(requestInfo.UserId);

                //Find Menu with same key.
                var letMe = menuPermissions.FirstOrDefault(x => x.MenuId == menu.Key);

                if (letMe == null)
                    passed = false;
                else
                {
                    if (menu.EditUrl.Eq(baseUrl) && (menu.EditUrl.Eq(menu.AddUrl) || IsUpdate(filterContext)) && !letMe.Edit ||
                        menu.AddUrl.Eq(baseUrl) && !letMe.Add ||
                        menu.DeleteUrl.Eq(baseUrl) && !letMe.Delete)
                        passed = false;
                }

            }

            //Check operator has permission.
            if (passed)
                base.OnActionExecuting(filterContext);
            else
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                    filterContext.Result = new JsonResult
                    {
                        Data = new AjaxResult("شما دسترسی لازم جهت انجام این عملیات را ندارید."),
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                else
                {
                    filterContext.RouteData.Values["controller"] = "Home";
                    filterContext.RouteData.Values["action"] = "Index";
                    filterContext.Controller.TempData["Msg"] = "شما دسترسی لازم جهت انجام این عملیات را ندارید";
                    filterContext.Controller.ViewData.Model = new DashboardViewModel();
                }
            }
        }

        private List<MenuPermissionViewModel> GetOperatorPermission(long operatorId)
        {
            if (HttpContext.Current.Session[AppConst.SessionKeys.OperatorPermission] == null)
                HttpContext.Current.Session[AppConst.SessionKeys.OperatorPermission] = _userService.GetOperatorPermissions(operatorId);
            return (List<MenuPermissionViewModel>)HttpContext.Current.Session[AppConst.SessionKeys.OperatorPermission] ?? new List<MenuPermissionViewModel>();
        }

        private static bool IsUpdate(ActionExecutingContext filterContext)
        {
            return (filterContext.HttpContext?.Request.Form["id"] ?? "").IsLong(0) > 0;
        }

    }

    public class IgnorePermissionAttribute : ActionFilterAttribute
    {

        // Just use name of SetRequestInfo as Action Attribute. By AHMAD.R
    }
}