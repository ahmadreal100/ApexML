using System.ComponentModel.DataAnnotations;
using Apex.Service.CustomAttribute.PropertyValidation;

namespace Apex.Service.ViewModels.Account
{
    public class ResetPasswordViewModel
    {

        [Required(ErrorMessage = "رمز عبور فعلی را وارد نمایید.")]
        [Display(Name = "رمز عبور فعلی")]
        public string CurrnetPassword { get; set; }

        [Required(ErrorMessage = "رمز عبور جدید را وارد نمایید.")]
        [Display(Name = "رمز عبور جدید")]
        [NotEqual("CurrnetPassword",ErrorMessage = "رمز جدید نمیتواند با رمز فعلی یکسان باشد.")]
        public string NewPassword { get; set; }

        [Display(Name = "تکرار رمز عبور جدید")]
        [Required(ErrorMessage = "تکرار رمز عبور جدید را وارد نمایید.")]
        [Compare("NewPassword", ErrorMessage = "رمز عبور جدید با تکرار آن مغایرت دارد.")]
        public string ConfirmNewPassword { get; set; }

        public bool WrongHashedPassword { get; set; } = false;
    }
}