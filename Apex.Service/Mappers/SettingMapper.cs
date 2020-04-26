using System.Collections.Generic;
using System.Linq;
using Apex.Core.Entities.LocaleE;
using Apex.Core.Entities.UserE;
using Apex.DAL.Helpers;
using Apex.Service.ViewModels.Setting;
using AutoMapper;

namespace Apex.Service.Mappers
{
    public class SettingMapper : Profile
    {
        private static bool SNull => SessionHelper.Session != null;
        private static Language Lng => SNull ? CultureHelper.CurrentLanguage() : new Language();
        public SettingMapper() : this("setting")
        {
            CreateMap<ThemeSettingViewModel, ThemeSetting>()
                .ForMember(x => x.Translations, y => y.MapFrom(x => new List<ThemeSettingTranslation> { Mapper.Map<ThemeSettingTranslation>(x) }));

            CreateMap<ThemeSettingViewModel, ThemeSettingTranslation>()
                .ForMember(x => x.LanguageId, y => y.MapFrom(x => Lng.Id));

            CreateMap<ThemeSetting, ThemeSettingViewModel>()
                .ForMember(x => x.CopyrightText, y => y.MapFrom(x => x.Translations.OrderByDescending(a => a.LanguageId == Lng.Id).FirstOrDefault().CopyrightText))
                .ForMember(x => x.HtmlContent, y => y.MapFrom(x => x.Translations.OrderByDescending(a => a.LanguageId == Lng.Id).FirstOrDefault().HtmlContent))
                .ForMember(x => x.FullDescription, y => y.MapFrom(x => x.Translations.OrderByDescending(a => a.LanguageId == Lng.Id).FirstOrDefault().FullDescription))
                .ForMember(x => x.FooterText, y => y.MapFrom(x => x.Translations.OrderByDescending(a => a.LanguageId == Lng.Id).FirstOrDefault().FooterText))
                .ForMember(x => x.PrivacyText, y => y.MapFrom(x => x.Translations.OrderByDescending(a => a.LanguageId == Lng.Id).FirstOrDefault().PrivacyText))
                .ForMember(x => x.LawsText, y => y.MapFrom(x => x.Translations.OrderByDescending(a => a.LanguageId == Lng.Id).FirstOrDefault().LawsText));
        }

        protected SettingMapper(string profileName) : base(profileName)
        {
        }

    }
}
