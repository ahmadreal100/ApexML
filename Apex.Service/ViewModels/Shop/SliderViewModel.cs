using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Apex.Service.ViewModels.Shop
{
    public class SliderViewModel
    {
        public long Id { get; set; }
        //[Display(Title = "موقعیت"), Required(ErrorMessage = "موقعیت اسلایدر را وارد نمایید.")]
        //public int Location { get; set; }
        public List<SliderPictureViewModel> Pictures { get; set; }
        public List<SliderPictureViewModel> OrderedPictures => Pictures?.OrderByDescending(x => x.DisplayOrder).ToList();
    }

    public class SliderPictureViewModel
    {
        public string Link { get; set; }
        public string LinkOld { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class TagViewModel
    {
        public long ProductId { get; set; }
        [Display(Name = "تگ")]
        public string Title { get; set; }
    }
}