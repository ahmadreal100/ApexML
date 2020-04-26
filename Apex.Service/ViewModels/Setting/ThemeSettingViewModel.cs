using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Apex.Service.ViewModels.Setting
{
    public class ThemeSettingViewModel
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        [Display(Name = "درباره شرکت")]
        [AllowHtml]
        public string FullDescription { get; set; }
        [Display(Name = "محتوای صفحه اصلی")]
        [AllowHtml]
        public string HtmlContent { get; set; }
        [Display(Name = "متن فوتر")]
        public string FooterText { get; set; }
        [Display(Name = "متن کپی رایت")]
        public string CopyrightText { get; set; }
        public bool UnderConstruction { get; set; }


        [AllowHtml]
        [Display(Name = "حریم خصوصی")]
        public string PrivacyText { get; set; }
        [AllowHtml]
        [Display(Name = "قوانین")]
        public string LawsText { get; set; }

    }
}
