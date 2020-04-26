using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Apex.Service.Abstracts;
using Apex.Service.Extensions;
using Apex.Service.ViewModels.Setting;
using Apex.Web.Controllers.Base;
using Apex.Web.Models;

namespace Apex.Web.Controllers.Content
{
    public class SettingController : BaseController
    {
        private readonly IUserService _userService;

        public SettingController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ActionResult> Index()
        {
            var model = new SettingViewModel
            {
                ThemeSetting = await _userService.GetThemeSettingAsync(),
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateTheme(SettingViewModel sModel)
        {
            try
            {
                var model = sModel.ThemeSetting;
                if (!ModelState.IsValid)
                    return Json(new AjaxResult(ModelState.JoinMessages()));

                var result = await _userService.SetThemeSetting(model);
                if (result.NotFound)
                    return Json(new AjaxResult("تنظیمات یافت نشد."));
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