using System.ComponentModel.DataAnnotations;

namespace Apex.Core.Enums
{
    public enum UserType : byte
    {
        [Display(Name = "کاربر")]
        User = 0,
        [Display(Name = "اپراتور")]
        Operator = 1,
        [Display(Name = "مدیر")]
        Admin = 2
    }
}
