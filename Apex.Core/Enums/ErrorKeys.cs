using System.ComponentModel.DataAnnotations;
// ReSharper disable InconsistentNaming

namespace Apex.Core.Enums
{
    public enum ErrorKeys
    {
        //not found
        [Display(Description = "حساب کل مورد نظر یافت نشد.")]
        CodingLedger_Not_Found = 4041000,
        [Display(Description = "گروه حساب مورد نظر یافت نشد.")]
        CodingGroup_Not_Found = 4041001,
        [Display(Description = "حساب معین مورد نظر یافت نشد.")]
        CodingSubsidiary_Not_Found = 4041002
    }
}
