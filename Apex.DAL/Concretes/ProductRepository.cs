using System.Linq;
using Apex.Core;
using Apex.Core.Entities.ShopE;
using Apex.DAL.Abstracts;
using Apex.DAL.EF;

namespace Apex.DAL.Concretes
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ILanguageRepository _languageRepository;
        public ProductRepository(ApexContext context,
            IUnitOfWork unitOfWork, RequestInfo info, ILanguageRepository languageRepository)
            : base(context, unitOfWork, info)
        {
            _languageRepository = languageRepository;
        }
        public void ClearTranslation(long productId)
        {
            var activeLangId = _languageRepository.ActiveId();
            var translation = Context.ProductTranslations.FirstOrDefault(m => m.ProductId == productId && m.LanguageId == activeLangId);
            if (translation != null)
                Context.ProductTranslations.Remove(translation);
        }
        public void ClearTags(long productId)
        {
            var activeLangId = _languageRepository.ActiveId();
            var pins = Context.Tags.Where(m => m.ProductId == productId && m.LanguageId == activeLangId);
            Context.Tags.RemoveRange(pins);
        }
        public void ClearPictures(long productId)
        {
            var pins = Context.ProductPictures.Where(m => m.ProductId == productId);
            Context.ProductPictures.RemoveRange(pins);
        }
    }
}
