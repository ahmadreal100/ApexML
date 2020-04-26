using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Apex.Core;
using Apex.Core.Entities.LocaleE;
using Apex.Core.Entities.UserE;
using Apex.DAL.Helpers;
using Apex.Service.Abstracts;
using Apex.Service.ViewModels.Account;
using Apex.Shared.Helpers;
using Apex.Web.Helpers;
using Apex.Web.Infrastructure;

namespace Apex.Web.Controllers.Base
{
    [AjaxHandler(Order = 1)]
    [PermissionHandler(Order = 2)]
    public class BaseController : Controller
    {
        private readonly IUserService _userService;
        protected readonly RequestInfo RequestInfo;
        protected Language ActiveLang => CultureHelper.CurrentLanguage();
        public BaseController()
        {
            _userService = (IUserService)DependencyResolver.Current.GetService(typeof(IUserService));
            RequestInfo = _userService.RequestInfo;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {

                var requestInfo = _userService.RequestInfo;
                var allMenus = OptPermission.LayoutMenu(Url).WhereNested(x => !x.Hide);

                var allowedMenus = allMenus;
                if (requestInfo.IsOperator)
                {
                    var permissions = GetOperatorPermissions(requestInfo.UserId);
                    var menuPermissions = permissions.Select(x => x.MenuId);

                    ViewBag.OperatorPermission = permissions;
                    allowedMenus = allMenus.WhereNested(x => menuPermissions.Contains(x.Key) || x.Execlude).ToList();
                }

                ViewBag.AllowedMenus = allowedMenus;
            }

            ViewBag.RequestInfo = _userService.RequestInfo;
            base.OnActionExecuting(filterContext);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
                if (User.Identity.IsAuthenticated)
                {
                    ViewBag.UserInfo = GetActorInfo();
                }

            if (filterContext.HttpContext.Request.IsAjaxRequest())
                if (filterContext.HttpContext.Request.Headers.AllKeys.Contains("keepLoading"))
                    KeepUiLoading();

            base.OnActionExecuted(filterContext);
        }

        protected string ViewToString(PartialViewResult result)
        {
            using (var sw = new StringWriter())
            {
                result.View = ViewEngines.Engines.FindPartialView(ControllerContext, result.ViewName).View;
                var vc = new ViewContext(ControllerContext, result.View, result.ViewData, result.TempData, sw);
                result.View.Render(vc, sw);
                return sw.GetStringBuilder().ToString();
            }
        }

        protected List<MenuPermissionViewModel> GetOperatorPermissions(long operatorId)
        {
            if (Session[AppConst.SessionKeys.OperatorPermission] == null)
                Session[AppConst.SessionKeys.OperatorPermission] = _userService.GetOperatorPermissions(operatorId);
            return (List<MenuPermissionViewModel>)Session[AppConst.SessionKeys.OperatorPermission] ?? new List<MenuPermissionViewModel>();
        }

        protected MasterInfo GetMasterUserInfo()
        {
            return RequestHelper.GetMasterInfo();
        }
        protected User GetActorInfo()
        {
            var user = _userService.Find(RequestInfo.UserId);
            user.MasterInfo = GetMasterUserInfo();
            return user;
        }

        protected void KeepUiLoading()
        {
            Response.Headers.Add("keepLoading", "true");
        }

        protected JsonResult JsonGet(object data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}