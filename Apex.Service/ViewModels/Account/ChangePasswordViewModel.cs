using System.ComponentModel.DataAnnotations;

namespace Apex.Service.ViewModels.Account
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "رمز فعلی را وارد نمایید.")]
        [Display(Name = "رمز فعلی")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "رمز جدید را وارد نمایید.")]
        [MinLength(6, ErrorMessage = "حداقل تعداد کاراکترهای رمز جدید 6 کاراکتر میباشد.")]
        [Display(Name = "رمز جدید")]
        public string NewPassword { get; set; }

        [Display(Name = "تکرار رمز جدید")]
        [Required(ErrorMessage = "تکرار رمز جدید را وارد نمایید.")]
        [Compare("NewPassword", ErrorMessage = "رمز جدید با تکرار رمز مغایرت دارد.")]
        public string ConfirmNewPassword { get; set; }
    }
}