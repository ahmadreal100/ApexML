using System.ComponentModel.DataAnnotations;
using Apex.Service.Translations;
using Apex.Shared.Helpers;

namespace Apex.Service.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "نام کاربری را وارد نمایید.")]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "رمز عبور را وارد نمایید.")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
    }
    public class LogInDto
    {
        [Required(ErrorMessage = nameof(Str.rq))]
        [RegularExpression(AppConst.RegexPattern.Mobile, ErrorMessage = nameof(Str.nv))]
        [Display(Name = nameof(Str.mobileNumebr))]
        public string Mobile { get; set; }

        [Required(ErrorMessage = nameof(Str.rq))]
        [Display(Name = nameof(Str.password))]
        public string Password { get; set; }

        [Display(Name = nameof(Str.rememberMe))]
        public bool RememberMe { get; set; }
    }

    public class SiteLoginViewModel
    {
        [Required(ErrorMessage = "شماره موبایل را وارد نمایید.")]
        [RegularExpression(@"09\d{9}", ErrorMessage = "شماره وارد شده نامعتبر میباشد.")]
        [Display(Name = "شماره موبایل")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "رمز عبور را وارد نمایید.")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
    }

    public class IdentityViewModels
    {
        public SiteLoginViewModel Login { get; set; }
        public RegisterViewModel Register { get; set; }

    }
}