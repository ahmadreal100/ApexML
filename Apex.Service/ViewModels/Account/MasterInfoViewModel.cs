using System.ComponentModel.DataAnnotations;
using Apex.Core.Entities.UserE;
using AutoMapper;

namespace Apex.Service.ViewModels.Account
{
    public class MasterInfoViewModel
    {
        public long Id { get; set; }
        [Display(Name = "نام")]
        public string ManagerName { get; set; }

        [Display(Name = "نام خانوادگی")]
        public string ManagerFamily { get; set; }

        [Display(Name = "عنوان کسب و کار")]
        public string BusinessName { get; set; }
        [Display(Name = "نام تجاری")]
        public string CommercialName { get; set; }
        [Display(Name = "تصویر لوگو")]
        public string LogoLink { get; set; }
        public string LogoLinkOld { get; set; }


        [Display(Name = "شماره تلفن"), RegularExpression("\\d+", ErrorMessage = "شماره تلفن وارد شده صحیح نیست")]
        public string PhoneNumber { get; set; }

        [Display(Name = "آدرس")]
        public string Address { get; set; }

        [Display(Name = "ایمیل")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمیباشد.")]
        public string Email { get; set; }

        [Display(Name = "کد پستی"), RegularExpression("\\d{10}", ErrorMessage = "کد پستی باید یک عدد 10 رقمی باشد.")]
        public string PostalCode { get; set; }

        [Display(Name = "شماره موبایل"),
         RegularExpression("09\\d{9}", ErrorMessage = "شماره موبایل وارد شده صحیح نیست")]
        public string Mobile { get; set; }

        [Display(Name = "شهر")]
        public long? CityId { get; set; }
        [Display(Name = "استان")]
        public long? ProvinceId { get; set; }


        public static MasterInfoViewModel Map(User user)
        {
            var tempProfile = Mapper.Map<MasterInfoViewModel>(user);
            var profile = Mapper.Map(user.MasterInfo ?? new MasterInfo(), tempProfile);
            return profile;
        }
    }
}