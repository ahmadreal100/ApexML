using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Apex.Core;
using Apex.Core.Entities.ShopE;
using Apex.Core.Entities.UserE;
using Apex.DAL.Abstracts;
using Apex.DAL.Helpers;
using Apex.Service.Abstracts;
using Apex.Service.ViewModels.Account;
using Apex.Service.ViewModels.Setting;
using Apex.Service.ViewModels.Shop;
using AutoMapper.QueryableExtensions;

namespace Apex.Web.Areas.Theme1.Controllers
{
    public class BaseController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRepository<Category> _categoryRepository;
        protected readonly RequestInfo RequestInfo;
        public BaseController()
        {
            _userService = (IUserService)DependencyResolver.Current.GetService(typeof(IUserService));
            _categoryRepository = _userService.UnitOfWork.Repository<Category>();
            RequestInfo = _userService.RequestInfo;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                ViewBag.SiteSetting = new SettingViewModel();
            }
            base.OnActionExecuting(filterContext);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                if (User.Identity.IsAuthenticated)
                {
                    ViewBag.UserInfo = GetUserInfo();
                }

                var master = RequestHelper.GetMasterUser();
                ViewBag.SiteSetting = new SettingViewModel
                {
                    BaseUserInfo = MasterInfoViewModel.Map(master),
                    ThemeSetting = _userService.GetThemeSetting()
                };
            }

            base.OnActionExecuted(filterContext);
        }

        protected User GetUserInfo()
        {
            return _userService.Find(RequestInfo.UserId) ?? new User();
        }

        protected string RenderViewToString(PartialViewResult result)
        {
            using (var sw = new StringWriter())
            {
                result.View = ViewEngines.Engines.FindPartialView(ControllerContext, result.ViewName).View;
                var vc = new ViewContext(ControllerContext, result.View, result.ViewData, result.TempData, sw);
                result.View.Render(vc, sw);
                return sw.GetStringBuilder().ToString();
            }
        }
        protected List<CategoryViewModel> GetCategories(long? parentId = null)
        {
            return _categoryRepository.Queryable().Where(x => x.ParentId == parentId).Include(x => x.Translations).ProjectTo<CategoryViewModel>().ToList();
        }

        protected JsonResult JsonGet(object data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}