using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Apex.Core.Entities.FrontE;
using Apex.Core.Entities.LocaleE;
using Apex.Core.Entities.ShopE;
using Apex.DAL.Helpers;
using Apex.Service.ViewModels.Shop;

namespace Apex.Service.Mappers
{
    public class ShopMapper : Profile
    {
        private static bool SNull => SessionHelper.Session != null;
        private static Language Lng => SNull ? CultureHelper.CurrentLanguage() : new Language();
        public ShopMapper() : this("shop")
        {
            CreateMap<ProductViewModel, Product>()
                .ForMember(x => x.Translations, y => y.MapFrom(x => new List<ProductTranslation> { Mapper.Map<ProductTranslation>(x) }))
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.AddedDate, y => y.Ignore());

            CreateMap<ProductViewModel, ProductTranslation>()
                .ForMember(x => x.LanguageId, y => y.MapFrom(x => Lng.Id));

            CreateMap<TagViewModel, Tag>()
                .ForMember(x => x.LanguageId, y => y.MapFrom(x => Lng.Id));


            CreateMap<CommentViewModel, Comment>()
                .ForMember(x => x.AddedDate, y => y.Ignore())
                .ForMember(x => x.Id, y => y.Ignore());

            CreateMap<CommentDto, Comment>()
                .ForMember(x => x.AddedDate, y => y.Ignore())
                .ForMember(x => x.Id, y => y.Ignore());


            CreateMap<CategoryViewModel, Category>()
                .BeforeMap((v, c) => { c.Translations = new List<CategoryTranslation> { Mapper.Map<CategoryTranslation>(v) }; })
                .ForMember(x => x.Id, y => y.Ignore());

            CreateMap<CategoryViewModel, CategoryTranslation>()
                .ForMember(x => x.LanguageId, y => y.MapFrom(x => Lng.Id));
            //===============================================================

            CreateMap<Product, ProductViewModel>()
                .ForMember(x => x.Title, y => y.MapFrom(x => x.Translations.OrderByDescending(a => a.LanguageId == Lng.Id).FirstOrDefault().Title))
                .ForMember(x => x.FullDescription, y => y.MapFrom(x => x.Translations.OrderByDescending(a => a.LanguageId == Lng.Id).FirstOrDefault().FullDescription))

                .ForMember(x => x.Tags, y => y.MapFrom(x => x.Tags.OrderByDescending(a => a.LanguageId == Lng.Id).GroupBy(a => a.LanguageId)
                    .OrderByDescending(a => a.Key == Lng.Id).FirstOrDefault().ToList()));


            CreateMap<Tag, TagViewModel>();

            CreateMap<Comment, CommentViewModel>();


            CreateMap<Category, CategoryViewModel>()
                .ForMember(x => x.Name, y => y.MapFrom(x => x.Translations.OrderByDescending(a => a.LanguageId == Lng.Id).FirstOrDefault().Name))
                .ForMember(x => x.Childs, y => y.Ignore());


            CreateMap<ProductPicture, ProductPictureViewModel>().ReverseMap();


            CreateMap<Visit, VisitViewModel>();
            CreateMap<VisitViewModel, Visit>()
                .ForMember(x => x.Id, y => y.Ignore());


            CreateMap<ProductViewModel, ProductThumbUiModel>()
                .ForMember(x => x.PicLink, x => x.MapFrom(y => y.OrderedPicture.FirstOrDefault().Link ?? ""));
        }

        protected ShopMapper(string profileName) : base(profileName)
        {
        }

    }
}
