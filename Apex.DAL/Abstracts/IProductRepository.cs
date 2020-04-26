using Apex.Core.Entities.ShopE;

namespace Apex.DAL.Abstracts
{
    public interface IProductRepository : IRepository<Product>
    {
        void ClearTranslation(long productId);
        void ClearTags(long productId);
        void ClearPictures(long productId);
    }
}
