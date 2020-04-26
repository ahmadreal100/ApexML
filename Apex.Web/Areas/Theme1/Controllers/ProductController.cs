using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Apex.Service.Services.Shop;
using Apex.Service.ViewModels;
using Apex.Service.ViewModels.Shop;
using Apex.Shared.Extensions;
using Apex.Web.Areas.Theme1.Models.DataViewModels;
using Apex.Web.Models;
using AutoMapper.QueryableExtensions;

namespace Apex.Web.Areas.Theme1.Controllers
{
    public class ProductController : BaseController
    {
        private readonly ProductService _service;
        private readonly CategoryService _categoryService;

        public ProductController(ProductService service, CategoryService categoryService)
        {
            _service = service;
            _categoryService = categoryService;
        }

        [Route("product/{id:regex(^\\d+-?)}", Name = "Product")]
        public async Task<ActionResult> Index(string id)
        {
            var productId = id.GetId();
            var product = _service.Repository.Queryable().Where(x => x.Id == productId)
                .Include(x => x.Translations)
                .Include(x => x.Tags)
                .Include(x => x.Pictures).ProjectTo<ProductViewModel>().FirstOrDefault();

            if (product == null)
                return HttpNotFound();

            var model = new ProductDetailsViewData
            {
                Product = product,
                RelatedProducts = await _service.GetRelatedThumbs(productId).OrderByDescending(x => x.Id).Take(4)
                    .ToListAsync(),
                Categories = product.CategoryId.HasValue ? await _categoryService.GetParents(product.CategoryId.Value) : new List<CategoryUiModel>()
            };
            return View(model);
        }

        [Route("search", Name = "Search")]
        public async Task<ActionResult> Search(long? categoryId, string keyword)
        {
            var cats = new List<CategoryUiModel>();

            if (categoryId.HasValue)
                cats = await _categoryService.GetParents(categoryId.Value);

            var model = new SearchViewData
            {
                Keyword = keyword,
                Categories = cats
            };

            return View(model);
        }

        public async Task<ActionResult> GetProducts(ProductFilter filter, int top = 0, int skip = 0)
        {
            try
            {
                var query = await _service.GetThumbs(filter);
                var products = await query.OrderByDescending(x => x.Id).Skip(skip).Take(top).ToListAsync();

                var data = products.Select(x => RenderViewToString(PartialView("Product/_ProductThumb", x)));
                return JsonGet(new AjaxResult
                {
                    Status = true,
                    Data = data,
                    ExData = query.Count()
                });
            }
            catch (Exception)
            {
                return JsonGet(new AjaxResult());
            }
        }
    }
}