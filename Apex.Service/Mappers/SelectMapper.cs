using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Apex.Core.Entities.AddressE;
using Apex.Core.Entities.LocaleE;
using Apex.Core.Entities.ShopE;
using Apex.DAL.Helpers;

namespace Apex.Service.Mappers
{
    public class SelectMapper : Profile
    {
        private static bool SNull => SessionHelper.Session != null;
        private static Language Lng => SNull ? CultureHelper.CurrentLanguage() : new Language();
        public SelectMapper() : this("select")
        {


            CreateMap<Category, SelectListItem>()
                .ForMember(x => x.Value, y => y.MapFrom(x => x.Id))
                .ForMember(x => x.Text, y => y.MapFrom(x => x.Translations.OrderByDescending(a => a.LanguageId == Lng.Id).FirstOrDefault().Name));

            CreateMap<Province, SelectListItem>()
                .ForMember(x => x.Value, y => y.MapFrom(z => z.Id))
                .ForMember(x => x.Text, y => y.MapFrom(z => z.Name));

            CreateMap<City, SelectListItem>()
                .ForMember(x => x.Value, y => y.MapFrom(z => z.Id))
                .ForMember(x => x.Text, y => y.MapFrom(z => z.Name));
        }

        protected SelectMapper(string profileName) : base(profileName)
        {
        }

    }
}
