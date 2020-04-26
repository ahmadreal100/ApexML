using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Apex.Core.Abstract;
using Apex.Core.Entities.FrontE;
using Apex.Core.Entities.LocaleE;

namespace Apex.Core.Entities.ShopE
{
    public class Product : Entity
    {
        public Product()
        {
            Tags = new List<Tag>();
            Pictures = new List<ProductPicture>();
        }

        public Category Category { get; set; }
        public long? CategoryId { get; set; }
        public ICollection<ProductPicture> Pictures { get; set; }
        public ICollection<Tag> Tags { get; set; }

        //Translation
        public ICollection<ProductTranslation> Translations { get; set; }
    }

    //Translation
    public class ProductTranslation : Translation
    {

        [Key, Column(Order = 2)]
        public long ProductId { get; set; }
        public Product Product { get; set; }

        public string Title { get; set; }
        public string FullDescription { get; set; }
    }
}