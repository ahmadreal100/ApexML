using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Apex.Core.Entities.ShopE;
using Apex.Service.Enums;
using Apex.Service.Extensions;

namespace Apex.Service.ViewModels.Shop
{
    public class CategoryViewModel
    {
        public CategoryViewModel()
        {
            Childs = new List<Category>();
        }
        public long Id { get; set; }
        [Display(Name = "نام"), Required(ErrorMessage = "نام دسته بندی را وارد نمایید.")]
        public string Name { get; set; }
        public long? ParentId { get; set; }
        public List<Category> Childs { get; set; }
        public int ChildsCount { get; set; }
        public string Link => LinkCreator.Create(LinkType.Category, Id, Name);
    }
}
