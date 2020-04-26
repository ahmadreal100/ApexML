using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Apex.Core.Enums;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Apex.Service.Concretes;
using Apex.Service.Extensions;
using Apex.Service.ViewModels.Account;
using Apex.Shared.Helpers;
using Apex.Web.Controllers.Base;
using Apex.Web.Helpers;
using Apex.Web.Models;

namespace Apex.Web.Controllers.Account
{
    public class OperatorController : BaseController
    {
        private readonly UserService _service;


        public OperatorController(UserService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult IndexGrid()
        {
            var operators = _service.UserRepository.Assets.Where(x => x.UserName != AppConst.Auth.Admin).ProjectTo<UserViewModel>();

            return PartialView("_IndexGrid", operators.OrderByDescending(x => x.Id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddOrUpdate(UserViewModel model)
        {
            try
            {
                if (model.Type == UserType.Admin)
                    return Json(new AjaxResult("عملیات با مشکل مواجه گردید."));

                var isUpdate = model.Id > 0;
                if (isUpdate)
                    model.ClearErrorForUpdate(ModelState);

                if (!ModelState.IsValid)
                    return Json(new AjaxResult(ModelState.JoinMessages()));

                if (await _service.UserNameExist(model.UserName, model.Id))
                    return Json(new AjaxResult("این شماره موبایل قبلا ثبت شده است."));

                var result = isUpdate ? await _service.Update(model.Id, model) : await _service.Create(model);

                if (result.Succeeded)
                    return Json(new AjaxResult(true, "اطلاعات با موفقیت ذخیره گردید."));

                return Json(new AjaxResult(result.State.Errors.JoinMessages()));
            }
            catch (Exception e)
            {
                return Json(new AjaxResult(e.JoinMessages()));
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                var result = await _service.Delete(id);
                if (result.NotFound)
                    return Json(new AjaxResult("کاربر مورد نظر یافت نشد."));

                if (result.Succeeded)
                    return Json(new AjaxResult(true, "کاربر مورد نظر با موفقیت حذف شد."));

                return Json(new AjaxResult(result.State.Errors.JoinMessages()));
            }
            catch (Exception e)
            {
                return Json(new AjaxResult(e.JoinMessages()));
            }
        }

        public ActionResult Get(long id)
        {
            try
            {
                var user = _service.UserRepository.OneAsset(id);
                if (user == null)
                    return JsonGet(new AjaxResult("کاربر مورد نظر یافت نشد."));

                var model = Mapper.Map<UserViewModel>(user);

                ViewBag.IsUser = user.OperatorInfo == null;
                var viewResult = PartialView("Manage/_AddOperator", model);
                return JsonGet(new AjaxResult(true) { Data = ViewToString(viewResult) });
            }
            catch (Exception e)
            {
                return JsonGet(new AjaxResult(e.JoinMessages()));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(long id, string password)
        {
            try
            {
                var result = await _service.ChangePassword(id, password);
                if (result.NotFound)
                    return Json(new AjaxResult("کاربر مورد نظر یافت نشد."));

                if (result.Succeeded)
                    return Json(new AjaxResult(true, "اطلاعات با موفقیت ذخیره گردید."));

                return Json(new AjaxResult(result.State.Errors.JoinMessages()));
            }
            catch (Exception e)
            {
                return JsonGet(new AjaxResult(e.JoinMessages()));
            }
        }

        public ActionResult Permissions(long id)
        {
            var user = _service.UserRepository.Asset(id).Include(x => x.OperatorInfo).FirstOrDefault();
            var p = _service.GetOperatorPermissions(id);

            ViewBag.LayoutMenuList = OptPermission.LayoutMenu(Url).WhereNested(x => !x.Execlude);
            ViewBag.Operator = user;

            var model = Mapper.Map<List<MenuPermissionViewModel>>(p);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPermissions(long id, List<string> menuIds, List<string> addIds, List<string> editIds, List<string> deleteIds)
        {
            try
            {
                var model = menuIds?.Select(x => new MenuPermissionViewModel
                {
                    MenuId = x,
                    Add = addIds?.Contains(x) ?? false,
                    Edit = editIds?.Contains(x) ?? false,
                    Delete = deleteIds?.Contains(x) ?? false

                }).ToList() ?? new List<MenuPermissionViewModel>();

                var result = await _service.SetOperatorPermission(id, model);
                if (result.NotFound)
                    return Json(new AjaxResult("کاربر مورد نظر یافت نشد."));

                if (result.Succeeded)
                    return Json(new AjaxResult(true, "اطلاعات با موفقیت ذخیره گردید."));

                return Json(new AjaxResult(result.State.Errors.JoinMessages()));
            }
            catch (Exception e)
            {
                return Json(new AjaxResult(e.JoinMessages()));
            }
        }
    }
}