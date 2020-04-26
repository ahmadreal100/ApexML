using System.ComponentModel.DataAnnotations;
using Apex.Service.Translations;
using Apex.Shared.Helpers;

namespace Apex.Service.ViewModels.Account
{
    public class RegisterViewModel
    {
        public long Id { get; set; }
        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "نام و نام خانوادگی را وارد نمایید.")]
        public string FirstName { get; set; }

        [RegularExpression(AppConst.RegexPattern.Mobile, ErrorMessage = "شماره وارد شده نامعتبر میباشد.")]
        [Required(ErrorMessage = "شماره موبایل را وارد نمایید.")]
        [Display(Name = "شماره موبایل")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "رمز عبور را وارد نمایید.")]
        [MinLength(6, ErrorMessage = "حداقل کاکترهای رمز عبور 6 کاکاکتر میباشد.")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تکرار رمز عبور")]
        [Compare(nameof(Password), ErrorMessage = "تکرار رمز با رمز عبور مغایرت دارد.")]
        [Required(ErrorMessage = "تکرار رمز عبور را وارد نمایید.")]
        public string ConfirmPassword { get; set; }
    }
    public class RegisterDto
    {
        [RegularExpression(AppConst.RegexPattern.Mobile, ErrorMessage = nameof(Str.nv))]
        [Required(ErrorMessage = nameof(Str.rq))]
        [Display(Name = nameof(Str.mobileNumebr))]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = nameof(Str.rq))]
        [Display(Name = nameof(Str.fullName))]
        public string FirstName { get; set; }


        [MinLength(6, ErrorMessage = nameof(Str.minLength))]
        [Required(ErrorMessage = nameof(Str.rq))]
        [Display(Name = nameof(Str.password))]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = nameof(Str.notSamePass))]
        [Required(ErrorMessage = nameof(Str.rq))]
        [Display(Name = nameof(Str.confirmPassword))]
        public string ConfirmPassword { get; set; }
    }
}