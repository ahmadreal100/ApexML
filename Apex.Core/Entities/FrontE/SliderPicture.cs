using Apex.Core.ComplexTypes;

namespace Apex.Core.Entities.FrontE
{
    public class SliderPicture : PictureBase
    {
        public long SliderId { get; set; }
        public Slider Slider { get; set; }
    }
}