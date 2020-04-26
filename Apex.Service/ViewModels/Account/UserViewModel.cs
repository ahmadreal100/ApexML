using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Apex.Core.Enums;
using Apex.Service.Extensions;

namespace Apex.Service.ViewModels.Account
{
    public class UserViewModel
    {
        public long Id { get; set; }
        [Display(Name = "شماره موبایل"), Required(ErrorMessage = "شماره موبایل را وارد نمایید.")]
        [RegularExpression("^09\\d{9}$", ErrorMessage = "شماره موبایل نامعتبر میباشد.")]
        public string UserName { get; set; }
        [Display(Name = "نام")]
        public string FirstName { get; set; }
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }
        [Display(Name = "نوع")]
        public UserType Type { get; set; }
        [Display(Name = "رمز عبور"), Required(ErrorMessage = "رمز عبور را وارد نمایید.")]
        [MinLength(6, ErrorMessage = "حداقل تعداد کاراکترهای رمز عبور 6 کاراکتر میباشد.")]
        public string Password { get; set; }
        [Display(Name = "تکرار رمز عبور"), Required(ErrorMessage = "تکرار رمز را وارد نمایید.")]
        [System.ComponentModel.DataAnnotations.Compare(nameof(Password), ErrorMessage = "رمز عبور با تکرار رمز مغایرت دارد.")]
        public string ConfirmPassword { get; set; }

        public bool IsOperator => Type == UserType.Operator;

        public void ClearErrorForUpdate(ModelStateDictionary state)
        {
            var uiProps = new[] { nameof(Password), nameof(ConfirmPassword) };
            state.ClearError(x => uiProps.Any(a => x == a));
        }
    }

    public class OperatorInfoViewModel
    {
        public List<MenuPermissionViewModel> MenuPermissions { get; set; }
    }

    public class MenuPermissionViewModel
    {
        public string MenuId { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
    }
}
