using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Apex.Core.Entities.LocaleE;

namespace Apex.Core.Entities.UserE
{
    public class ThemeSetting
    {
        public User User { get; set; }
        [Key, ForeignKey("User")]
        public long UserId { get; set; }
        [StringLength(128)]
        public string Latitude { get; set; }
        [StringLength(128)]
        public string Longitude { get; set; }
        public bool UnderConstruction { get; set; }

        //Translation
        public ICollection<ThemeSettingTranslation> Translations { get; set; }
    }
    //Translation
    public class ThemeSettingTranslation : Translation
    {
        [Key, Column(Order = 2)]
        public long ThemeSettingUserId { get; set; }
        [ForeignKey("ThemeSettingUserId")]
        public ThemeSetting ThemeSetting { get; set; }
        public string FullDescription { get; set; }
        public string HtmlContent { get; set; }
        public string FooterText { get; set; }
        public string CopyrightText { get; set; }

        public string PrivacyText { get; set; }
        public string LawsText { get; set; }

    }
}