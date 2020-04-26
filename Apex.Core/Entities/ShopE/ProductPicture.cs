using Apex.Core.ComplexTypes;

namespace Apex.Core.Entities.ShopE
{
    public class ProductPicture : PictureBase
    {
        public long ProductId { get; set; }
        public Product Product { get; set; }
    }
}