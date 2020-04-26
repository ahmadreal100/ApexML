using System.Collections.Generic;
using Apex.Service.Enums;
using Apex.Service.Extensions;

namespace Apex.Service.ViewModels.Shop
{
    public class CategoryUiModel
    {
        public long Id { get; set; }
        public long? ParentId { get; set; }
        public string Name { get; set; }
        public string Link => LinkCreator.Create(LinkType.Category, Id, Name);

        public IEnumerable<CategoryUiModel> Childs { get; set; }
    }
}