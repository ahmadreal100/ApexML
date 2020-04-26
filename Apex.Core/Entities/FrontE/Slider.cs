using System.Collections.Generic;
using Apex.Core.Abstract;

namespace Apex.Core.Entities.FrontE
{
    public class Slider : Entity
    {
        public int Location { get; set; }
        public ICollection<SliderPicture> Pictures { get; set; }
    }
}