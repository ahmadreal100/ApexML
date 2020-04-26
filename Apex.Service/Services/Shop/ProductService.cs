using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Apex.Core;
using Apex.Core.Entities.ShopE;
using Apex.DAL.Abstracts;
using Apex.Service.Services.Base;
using Apex.Service.ViewModels;
using Apex.Service.ViewModels.Shop;
using Apex.Shared.Extensions;

namespace Apex.Service.Services.Shop
{
    public class ProductService : BaseService
    {
        public IProductRepository Repository { get; }
        public IRepository<ProductPicture> PictureRepository { get; }
        public ILanguageRepository LanguageRepository { get; }
        private readonly CategoryService _categoryService;

        public ProductService(RequestInfo info, IUnitOfWork unitOfWork
            , IProductRepository productRepository, IRepository<ProductPicture> pictureRepository, ILanguageRepository languageRepository, CategoryService categoryService) : base(unitOfWork, info)
        {
            Repository = productRepository;
            PictureRepository = pictureRepository;
            LanguageRepository = languageRepository;
            _categoryService = categoryService;
        }

        public IQueryable<ProductViewModel> GetAll => Repository.Assets.ProjectTo<ProductViewModel>();

        public ProductViewModel GetOne(long id)
        {
            return Repository.Assets.ProjectTo<ProductViewModel>().FirstOrDefault(x => x.Id == id);
        }
        public async Task<ServiceResult<ProductViewModel>> Create(ProductViewModel model)
        {
            var res = new ServiceResult<ProductViewModel>();
            var product = Mapper.Map<Product>(model);

            Repository.Insert(product);

            try
            {
                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                res.ServerError();
            }

            res.Result = Mapper.Map<ProductViewModel>(product);
            return res;
        }

        public async Task<ServiceResult<ProductViewModel>> Update(long id, ProductViewModel model)
        {
            var res = new ServiceResult<ProductViewModel>();

            var product = Repository.OneAsset(id);
            if (product == null)
            {
                res.NotFound = true;
                return res;
            }

            Mapper.Map(model, product);

            Repository.ClearTags(product.Id);
            Repository.ClearPictures(product.Id);
            LanguageRepository.RemoveProductFromLanguage(product.Id);

            Repository.Update(product);
            try
            {
                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                res.ServerError();
            }

            res.Result = model;
            return res;
        }

        public async Task<ServiceResult<Product>> Delete(long id)
        {
            var res = new ServiceResult<Product>();

            var product = Repository.OneAsset(id);
            if (product == null)
            {
                res.NotFound = true;
                return res;
            }

            Repository.Delete(product);
            try
            {
                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                res.ServerError();
            }
            res.Result = product;
            return res;
        }

        public async Task<IQueryable<ProductThumbUiModel>> GetThumbs(ProductFilter filter = null)
        {
            var lngId = CurrentLanguage.Id;

            var catIds = new List<long>();
            if (filter?.CategoryId != null)
                catIds = (await _categoryService.GetSubs(filter.CategoryId.Value,true))?.Select(x => x.Id).ToList() ?? new List<long>();

            var products = Repository.Queryable();
            if (catIds.Any())
                products = products.Where(x => catIds.Contains(x.CategoryId.Value));

            products = products.Include(x => x.Pictures)
            .Include(x => x.Translations);

            var items = from p in products
                        let pt = p.Translations.OrderByDescending(a => a.LanguageId == lngId).FirstOrDefault()
                        let pic = p.Pictures.OrderByDescending(a => a.DisplayOrder).FirstOrDefault()
                        select new ProductThumbUiModel
                        {
                            Id = p.Id,
                            Name = pt.Title,
                            PicLink = pic == null ? "" : pic.Link,
                            AddedDate = p.AddedDate,
                            CategoryId = p.CategoryId
                        };

            if (filter != null && !filter.Keyword.IsNeu())
                items = items.Where(x => x.Name.Contains(filter.Keyword));

            return items;
        }
        public IQueryable<ProductThumbUiModel> GetRelatedThumbs(long productId)
        {
            var lngId = CurrentLanguage.Id;
            var catId = Repository.OneAsset(productId)?.CategoryId;

            var gps = Repository.Queryable()
                .Where(x => x.Id != productId && x.CategoryId == catId)
                .Include(x => x.Pictures)
                .Include(x => x.Translations);
            var items = from p in gps
                        let pt = p.Translations.OrderByDescending(a => a.LanguageId == lngId).FirstOrDefault()
                        let pic = p.Pictures.OrderByDescending(a => a.DisplayOrder).FirstOrDefault()
                        select new ProductThumbUiModel
                        {
                            Id = p.Id,
                            Name = pt.Title,
                            PicLink = pic == null ? "" : pic.Link,
                            AddedDate = p.AddedDate
                        };

            return items;
        }
    }
}
