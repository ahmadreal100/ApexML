using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Apex.Core;
using Apex.Core.Entities.ShopE;
using Apex.DAL.Abstracts;
using Apex.Service.Services.Base;
using Apex.Service.ViewModels.Shop;

namespace Apex.Service.Services.Shop
{
    public class CategoryService : BaseService
    {
        public IRepository<Category> Repository { get; }
        public IProductRepository ProductRepository { get; }
        public IRepository<ProductPicture> PictureRepository { get; }
        public ILanguageRepository LanguageRepository { get; }

        public CategoryService(RequestInfo info, IUnitOfWork unitOfWork,
            IRepository<Category> repository, IProductRepository productRepository, IRepository<ProductPicture> pictureRepository, ILanguageRepository languageRepository) : base(unitOfWork, info)
        {
            ProductRepository = productRepository;
            PictureRepository = pictureRepository;
            LanguageRepository = languageRepository;
            Repository = repository;
        }
        public async Task<ServiceResult<Category>> Create(CategoryViewModel model)
        {
            var res = new ServiceResult<Category>();
            var dg = Mapper.Map<Category>(model);

            Repository.Insert(dg);
            await UnitOfWork.SaveChangesAsync();

            res.Result = dg;
            return res;
        }

        public async Task<ServiceResult<Category>> Update(long id, CategoryViewModel model)
        {

            var res = new ServiceResult<Category>();
            var dg = Repository.OneAsset(id);
            if (dg == null)
            {
                res.NotFound = true;
                return res;
            }

            Mapper.Map(model, dg);
            LanguageRepository.RemoveCategoryFromLanguage(dg.Id);
            Repository.Update(dg);
            try
            {
                await UnitOfWork.SaveChangesAsync();

                res.Result = dg;
                return res;
            }
            catch (Exception)
            {

                res.Result = dg;
                res.ServerError();
                return res;
            }
        }

        public async Task<ServiceResult<Category>> Delete(long id)
        {
            var res = new ServiceResult<Category>();
            var dg = await Repository.Assets.Include(x => x.Childs).FirstOrDefaultAsync(x => x.Id == id);
            if (dg == null)
            {
                res.NotFound = true;
                return res;
            }

            if (await ProductRepository.Assets.AnyAsync(x => x.CategoryId == id))
            {
                res.State.Errors.Add("1000", dg.Id, "");
                return res;
            }

            if (dg.Childs.Any())
            {
                res.State.Errors.Add("1001", dg.Id, "");
                return res;
            }

            Repository.Delete(dg);
            await UnitOfWork.SaveChangesAsync();
            return res;
        }

        public async Task<List<CategoryUiModel>> GetAllNested(long? parentId = null, bool withSelf = false)
        {
            var items = await All();

            items.ForEach(x => x.Childs = items.Where(c => c.ParentId == x.Id).ToList());
            return items.Where(x => withSelf ? x.Id == parentId : x.ParentId == parentId).ToList();
        }

        public async Task<List<CategoryUiModel>> GetParents(long id)
        {
            var items = await All();

            var cats = new List<CategoryUiModel>();
            void Set(long pid)
            {
                var cat = items.FirstOrDefault(x => x.Id == pid);
                if (cat == null) return;
                cats.Add(cat);
                if (cat.ParentId.HasValue)
                    Set(cat.ParentId.Value);

            }

            Set(id);
            cats.Reverse();
            return cats;
        }
        public async Task<List<CategoryUiModel>> GetSubs(long id, bool withSelf = false)
        {
            var parents = (await GetAllNested(id, withSelf) ?? new List<CategoryUiModel>());
            if (!parents.Any()) return new List<CategoryUiModel>();

            var cats = new List<CategoryUiModel>();
            void Set(List<CategoryUiModel> items)
            {
                cats.AddRange(items);
                items.ForEach(x =>
                {
                    if (x.Childs.Any())
                        Set(x.Childs.ToList());
                });
            }

            Set(parents);
            return cats.OrderBy(x => x.Id).ToList();
        }
        public async Task<List<CategoryUiModel>> All()
        {
            var lngId = CurrentLanguage.Id;

            var cs = Repository.Queryable().Include(x => x.Translations);
            var items = await (from c in cs
                               let t = c.Translations.OrderByDescending(a => a.LanguageId == lngId).FirstOrDefault()
                               select new CategoryUiModel
                               {
                                   Id = c.Id,
                                   Name = t.Name,
                                   ParentId = c.ParentId
                               }).ToListAsync();
            return items;
        }
    }
}
