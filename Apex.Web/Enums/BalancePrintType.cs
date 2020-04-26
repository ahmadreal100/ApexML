using System.ComponentModel.DataAnnotations;

namespace Apex.Web.Enums
{
    public enum PricingType
    {
        [Display(Name = "میانگین")]
        Average = 0,
        [Display(Name = "FIFO")]
        Fifo = 1
    }
}
