using Apex.Core.Entities.FrontE;

namespace Apex.DAL.Abstracts
{
    public interface ISliderRepository : IRepository<Slider>
    {
        void ClearPictures(long sliderId);
    }
}
