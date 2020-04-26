using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Apex.Core.Entities.AddressE;
using Apex.Service.Abstracts;
using Apex.Service.Extensions;
using Apex.Service.ViewModels.Account;
using Apex.Shared.Helpers;
using Apex.Web.Controllers.Base;
using Apex.Web.Helpers;
using Apex.Web.Infrastructure;
using Apex.Web.Models;
using AutoMapper.QueryableExtensions;

namespace Apex.Web.Controllers.Account
{
    [IgnorePermission]
    public class AccountController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly IUserService _service;
        private readonly IService<Province> _provinceService;
        private readonly IService<City> _cityService;
        public AccountController(IUserService service,
            IService<Province> provinceService,
            IService<City> cityService)
        {
            _service = service;
            _provinceService = provinceService;
            _cityService = cityService;
        }

        public AccountController(ApplicationUserManager userManager,
            ApplicationSignInManager signInManager,
            IService<Province> provinceService,
            IUserService service,
            IService<City> cityService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _provinceService = provinceService;
            _service = service;
            _cityService = cityService;
        }

        public ApplicationSignInManager SignInManager
        {
            get => _signInManager ?? HttpContext.GetOwinContext()
                       .Get<ApplicationSignInManager>();
            private set => _signInManager = value;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext()
                       .GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        public new ActionResult Profile()
        {
            var user = _service.List.Where(x => x.Id == RequestInfo.UserId).Include(x => x.MasterInfo.Translations).FirstOrDefault();
            //ViewBag.IsOperator = _service.RequestInfo.IsOperator;

            //if (_service.RequestInfo.IsOperator)
            //{
            //    return View(new UserProfileViewModel
            //    {
            //        Id = user.Id,
            //        ManagerName = user.ManagerName,
            //        ManagerFamily = user.ManagerFamily
            //    });

            //}

            var userProfile = MasterInfoViewModel.Map(user);

            var provinces = _provinceService.List.ProjectTo<SelectListItem>().ToList();
            if (userProfile?.CityId.HasValue ?? false)
            {
                var cities = _cityService.List.Where(x => x.ProvinceId == userProfile.ProvinceId.Value).ProjectTo<SelectListItem>().ToList();
                ViewBag.Cities = cities;
            }
            ViewBag.Provinces = provinces;

            return View(userProfile);
        }

        //
        // POST: /Account/Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
#pragma warning disable 109
        public new async Task<ActionResult> Profile(MasterInfoViewModel model)
#pragma warning restore 109
        {
            if (!ModelState.IsValid)
                return Json(new AjaxResult("اطلاعات ورودی نادرست میباشد."));

            model.LogoLink = FileHelper.CopyFile(model.LogoLink, AppConst.Folder.MasterInfo);

            var result = await _service.Update(RequestInfo.UserId, model);
            if (result.NotFound)
                return Json(new AjaxResult("کاربر مورد نظر یافت نشد."));

            if (result.Succeeded)
            {
                if (!string.Equals(model.LogoLink, model.LogoLinkOld, StringComparison.CurrentCultureIgnoreCase))
                    FileHelper.RemoveFile(model.LogoLinkOld);
                return Json(new AjaxResult(true, "تغییرات با موفقیت اعمال گردید.", result.Result));
            }

            return Json(new AjaxResult(result.State.Errors.JoinMessages()));


        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (Request.IsAuthenticated)
                return RedirectToLocal(returnUrl);

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            try
            {

                if (!ModelState.IsValid)
                    return Json(new AjaxResult("نام کاربری یا رمز عبور اشتباه است."));


                var success = false;

                var user = await UserManager.FindByNameAsync(model.UserName);

                if (user != null && UserManager.CheckPassword(user, model.Password))
                {
                    var identity = await SignInManager.CreateUserIdentityAsync(user);
                    identity.AddClaim(new Claim("UserType", ((int)user.Type).ToString()));
                    SignInManager.AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe
                    }, identity);
                    success = true;
                }

                if (success)
                    return Json(new AjaxResult(true));

            }
            catch
            {
                //   
            }
            return Json(new AjaxResult("نام کاربری یا رمز عبور اشتباه است."));
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new AjaxResult(ModelState.FirstOrDefault(x => x.Value.Errors.Any()).Value.Errors[0].ErrorMessage));

                var res = await _service.ResetPassword(model);
                if (res.NotFound)
                    return Json(new AjaxResult("کاربر مورد نظر یافت نشد."));

                if (res.Result.WrongHashedPassword)
                    return Json(new AjaxResult("رمز عبور فعلی نادرست میباشد."));

                if (!res.Succeeded)
                    return Json(new AjaxResult("عملیات با مشکل مواجه گردید"));

                AuthenticationManager.SignOut();
                return Json(new AjaxResult(true, "تغییرات با موفقیت اعمال گردید."));
            }
            catch (Exception e)
            {
                return Json(new AjaxResult(e.Message));

            }
        }

        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}