using AutoMapper;
using Apex.Core.Entities.FrontE;
using Apex.Service.ViewModels.Shop;

namespace Apex.Service.Mappers
{
    public class ContentMapper : Profile
    {
        public ContentMapper() : this("content")
        {


            CreateMap<SliderViewModel, Slider>()
                .ForMember(x => x.Id, y => y.Ignore());

            CreateMap<Slider, SliderViewModel>();

            CreateMap<SliderPicture, SliderPictureViewModel>();
            CreateMap<SliderPictureViewModel, SliderPicture>()
                .ForMember(x => x.Id, y => y.Ignore());
        }

        protected ContentMapper(string profileName) : base(profileName)
        {
        }

    }
}
