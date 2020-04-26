using System.Linq;
using Apex.Core;
using Apex.Core.Entities.FrontE;
using Apex.DAL.Abstracts;
using Apex.DAL.EF;

namespace Apex.DAL.Concretes
{
    public class SliderRepository : Repository<Slider>, ISliderRepository
    {

        public SliderRepository(ApexContext context,
            IUnitOfWork unitOfWork, RequestInfo info)
            : base(context, unitOfWork, info)
        {
        }

        public void ClearPictures(long sliderId)
        {
            var pins = Context.SliderPictures.Where(m => m.SliderId == sliderId);
            Context.SliderPictures.RemoveRange(pins);
        }
    }
}
