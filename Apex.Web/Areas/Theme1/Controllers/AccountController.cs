using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Apex.Core.Entities.UserE;
using Apex.Core.Enums;
using Apex.Service.Abstracts;
using Apex.Service.Extensions;
using Apex.Service.Translations;
using Apex.Service.ViewModels.Account;
using Apex.Web.Infrastructure;
using Apex.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Apex.Web.Areas.Theme1.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        public AccountController(ApplicationUserManager userManager,
            ApplicationSignInManager signInManager,
            IUserService userService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _userService = userService;
        }

        public ApplicationSignInManager SignInManager
        {
            get => _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            private set => _signInManager = value;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        [AllowAnonymous]
        [Route("login", Name = "LogIn")]
        public ActionResult Login(string returnUrl)
        {
            if (Request.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoginConfirm(LogInDto model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return Json(new AjaxResult(Str.incorrectData));

            var user = await UserManager.FindByNameAsync(model.Mobile);
            if (user != null && user.Type == UserType.User && UserManager.CheckPassword(user, model.Password))
            {
                //if (!user.PhoneNumberConfirmed)
                //    return Json(new AjaxResult { Status = false, Message = "حساب کاربری شما فعال نشده است." });

                var identity = await SignInManager.CreateUserIdentityAsync(user);
                SignInManager.AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = model.RememberMe, AllowRefresh = true }, identity);
                return Json(new AjaxResult(true,Str.operationSuccessful));
            }
            return Json(new AjaxResult(Str.isIncorrect.Ft($"{Str.userName} {Str.or} {Str.password}")));
        }

        [AllowAnonymous]
        [Route("register", Name = "Register")]
        public ActionResult Register()
        {
            if (Request.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ArCaptcha]
        public async Task<ActionResult> RegisterConfirm(RegisterDto model)
        {
            if (!ModelState.IsValid)
                return Json(new AjaxResult(ModelState.JoinMessages()));

            var user = new User
            {
                UserName = model.PhoneNumber,
                FirstName = model.FirstName,
                PhoneNumber = model.PhoneNumber,
                PasswordHash = model.Password,
            };

            if (await _userService.List.AnyAsync(x => x.UserName == user.UserName))
                return Json(new AjaxResult(Str.already.Ft(Str.mobileNumebr)));

            var result = _userService.CreateUser(user);
            if (result.Succeeded)
            {
                await SignInManager.SignInAsync(user, true, true);
                return Json(new AjaxResult(true, Str.operationSuccessful));
            }
            return Json(new AjaxResult(Str.operationFailed));
        }

        [Route("logout", Name = "LogOut")]
        public ActionResult LogOut(string returnUrl)
        {
            SignOut();
            return RedirectToLocal(returnUrl);
        }

        private void SignOut()
        {
            SignInManager.AuthenticationManager.SignOut();
            Session.Abandon();
            Session.Clear();
        }

        //public new ActionResult Profile()
        //{
        //    var user = GetUserInfo();
        //    var mu = Mapper.Map<RegisterViewModel>(user);

        //    var model = new ProfileViewModel
        //    {
        //        UserInfo = mu,
        //    };
        //    return View(model);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateUserInfo(ProfileViewModel model)
        {
            try
            {
                //model.ClearErrorForUseInfo(ModelState);

                if (!ModelState.IsValid)
                    return Json(new AjaxResult { Status = false, Message = ModelState.JoinMessages() });

                var result = await _userService.UpdateUserInfo(model.UserInfo);

                if (result.Succeeded)
                    return Json(new AjaxResult { Status = true, Message = "اطلاعات با موفقیت ذخیره گردید." });

                return Json(new AjaxResult { Status = false, Message = result.State.Errors.JoinMessages() });

            }
            catch (Exception e)
            {
                return Json(new AjaxResult { Status = false, Message = e.JoinMessages() });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new AjaxResult { Status = false, Message = ModelState.JoinMessages() });

                var result = await _userService.ChangeUserPassword(GetUserInfo().Id, model);

                if (result.Succeeded)
                {
                    SignOut();
                    return Json(new AjaxResult { Status = true, Message = "اطلاعات با موفقیت ذخیره گردید. لطفا مجددا وارد شوید." });
                }

                return Json(new AjaxResult { Status = false, Message = result.State.Errors.JoinMessages() });

            }
            catch (Exception e)
            {
                return Json(new AjaxResult { Status = false, Message = e.JoinMessages() });
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> UserNameExist([Bind(Prefix = "Register.PhoneNumber")]string username, [Bind(Prefix = "Register.Id")]long id = 0)
        {
            return Json(!await _userService.UserNameExist(username, id));
        }

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