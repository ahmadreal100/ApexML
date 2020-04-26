using System.Collections.Generic;
using System.Linq;
using Apex.Service.Enums;
using Apex.Service.Extensions;

namespace Apex.Service.ViewModels.Shop
{
    public class ProductThumbViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal Discount { get; set; }

        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public List<ProductPictureViewModel> Pictures { get; set; }
        public string Picture => Pictures?.OrderByDescending(x => x.DisplayOrder).FirstOrDefault()?.Link;

        public List<ProductCategoryPinViewModel> Categories { get; set; }

        public string Link => LinkCreator.Create(LinkType.Product, Id, Name);
    }
}