using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Apex.Core.Abstract;
using Apex.Core.Entities.LocaleE;

namespace Apex.Core.Entities.ShopE
{
    public class Category : Entity
    {
        public long? ParentId { get; set; }
        public Category Parent { get; set; }
        public ICollection<Category> Childs { get; set; }
        public ICollection<CategoryTranslation> Translations { get; set; }
    }
    //Translation
    public class CategoryTranslation : Translation
    {
        [Key, Column(Order = 2)]
        public long CategoryId { get; set; }
        public Category Category { get; set; }
        public string Name { get; set; }
    }
}