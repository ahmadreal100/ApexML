using System.Collections.Generic;
using System.Linq;
using Apex.Core.Entities.LocaleE;
using Apex.Core.Entities.UserE;
using Apex.DAL.Helpers;
using Apex.Service.ViewModels.Account;
using AutoMapper;

namespace Apex.Service.Mappers
{
    public class ProfileMapper : Profile
    {
        private static bool SNull => SessionHelper.Session != null;
        private static Language Lng => SNull ? CultureHelper.CurrentLanguage() : new Language();
        public ProfileMapper() : this("profile")
        {
            CreateMap<MasterInfoViewModel, User>().ForMember(x => x.Id, y => y.Ignore());
            CreateMap<MasterInfoViewModel, MasterInfo>()
                .ForMember(x => x.Translations, y => y.MapFrom(x => new List<MasterInfoTranslation> { Mapper.Map<MasterInfoTranslation>(x) }));

            CreateMap<MasterInfoViewModel, MasterInfoTranslation>()
                .ForMember(x => x.LanguageId, y => y.MapFrom(x => Lng.Id));

            CreateMap<User, MasterInfoViewModel>();
            CreateMap<MasterInfo, MasterInfoViewModel>()
                .ForMember(x => x.ManagerName, y => y.MapFrom(x => x.Translations.OrderByDescending(a => a.LanguageId == Lng.Id).FirstOrDefault().ManagerName))
                .ForMember(x => x.ManagerFamily, y => y.MapFrom(x => x.Translations.OrderByDescending(a => a.LanguageId == Lng.Id).FirstOrDefault().ManagerFamily))
                .ForMember(x => x.Address, y => y.MapFrom(x => x.Translations.OrderByDescending(a => a.LanguageId == Lng.Id).FirstOrDefault().Address))
                .ForMember(x => x.BusinessName, y => y.MapFrom(x => x.Translations.OrderByDescending(a => a.LanguageId == Lng.Id).FirstOrDefault().BusinessName))
                .ForMember(x => x.CommercialName, y => y.MapFrom(x => x.Translations.OrderByDescending(a => a.LanguageId == Lng.Id).FirstOrDefault().CommercialName));

            CreateMap<OperatorInfo, OperatorInfoViewModel>();
            CreateMap<OperatorInfoViewModel, OperatorInfo>();

            CreateMap<User, MasterInfoViewModel>()
                .ForMember(x => x.ManagerName, y => y.MapFrom(x => x.FirstName));
        }

        protected ProfileMapper(string profileName) : base(profileName)
        {
        }

    }
}
