using System.Collections.Generic;
using Apex.Service.ViewModels.Shop;

namespace Apex.Web.Areas.Theme1.Models.DataViewModels
{
    public class IndexViewData
    {
        public IndexViewData()
        {
            LastProducts = new List<ProductThumbUiModel>();
        }
        public SliderViewModel Slider0 { get; set; }
        public List<ProductThumbUiModel> LastProducts { get; set; }
    }
    public class ProductDetailsViewData
    {
        public ProductViewModel Product { get; set; }
        public List<ProductThumbUiModel> RelatedProducts { get; set; }
        public List<CategoryUiModel> Categories { get; set; } = new List<CategoryUiModel>();
    }
    public class SearchViewData
    {
        public string Keyword { get; set; }
        public List<CategoryUiModel> Categories { get; set; }
    }
}