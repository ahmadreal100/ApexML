using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Apex.Core.Entities.ShopE;
using Apex.DAL.Abstracts;
using AutoMapper.QueryableExtensions;
using Apex.Service.Extensions;
using Apex.Service.Services.Shop;
using Apex.Service.ViewModels.Shop;
using Apex.Shared.Helpers;
using Apex.Web.Controllers.Base;
using Apex.Web.Helpers;
using Apex.Web.Models;

namespace Apex.Web.Controllers.Shop
{
    public class ProductController : BaseController
    {
        private readonly ProductService _service;
        private readonly IRepository<Category> _categoryRepository;

        public ProductController(ProductService service, IRepository<Category> categoryRepository)
        {
            _service = service;
            _categoryRepository = categoryRepository;
        }

        public ActionResult Index()
        {
            SetViewBags();
            return View();
        }


        [HttpGet]
        public PartialViewResult IndexGrid()
        {
            var products = _service.GetAll;
            return PartialView("_IndexGrid", products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddOrUpdate(ProductViewModel model)
        {
            try
            {
                model.SetTags();
                if (!ModelState.IsValid)
                    return Json(new AjaxResult(ModelState.JoinMessages()));

                var isUpdate = model.Id > 0;

                model.Pictures?.ForEach(x => x.Link = FileHelper.CopyFile(x.Link, AppConst.Folder.Product));
                if (isUpdate)
                {
                    var pics = _service.Repository.Asset(model.Id).SelectMany(x => x.Pictures).ToList();
                    pics.ForEach(x =>
                    {
                        if (!model.Pictures.Select(a => a.Link).Contains(x.Link))
                            FileHelper.RemoveFile(x.Link);
                    });
                }
                var result = isUpdate ? await _service.Update(model.Id, model) : await _service.Create(model);
                if (result.NotFound)
                    return Json(new AjaxResult("مطلب مورد نظر یافت نشد."));

                if (result.Succeeded)
                {
                    model.Pictures?.ForEach(x =>
                    {
                        if (!string.Equals(x.Link, x.LinkOld, StringComparison.CurrentCultureIgnoreCase))
                            FileHelper.RemoveFile(x.LinkOld);
                    });
                    return Json(new AjaxResult(true, "اطلاعات با موفقیت ذخیره گردید."));
                }

                return Json(new AjaxResult(result.State.Errors.JoinMessages()));
            }
            catch (Exception e)
            {
                return Json(new AjaxResult(e.JoinMessages()));
            }
        }


        [HttpPost]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                var pictures = _service.Repository.Asset(id).SelectMany(x => x.Pictures).ToList();
                var result = await _service.Delete(id);
                if (result.NotFound)
                    return Json(new AjaxResult("مطلب مورد نظر یافت نشد."));

                if (result.Succeeded)
                {
                    pictures.ForEach(x => FileHelper.RemoveFile(x.Link));
                    return Json(new AjaxResult(true, "مطلب مورد نظر با موفقیت حذف شد."));
                }
                return Json(new AjaxResult(result.State.Errors.JoinMessages()));

            }
            catch (Exception e)
            {
                return Json(new AjaxResult(e.JoinMessages()));
            }
        }

        public ActionResult Get(long id)
        {
            try
            {
                var product = _service.Repository.Asset(id).ProjectTo<ProductViewModel>().FirstOrDefault();
                if (product == null)

                    return JsonGet(new AjaxResult("مطلب مورد نظر یافت نشد."));

                SetViewBags();

                product.SetTagsInput();
                var viewResult = PartialView("Shop/_AddProduct", product);
                return JsonGet(new AjaxResult(true) { Data = ViewToString(viewResult) });
            }
            catch (Exception e)
            {
                return JsonGet(new AjaxResult(e.JoinMessages()));
            }
        }

        private void SetViewBags()
        {
            ViewBag.Categories = _categoryRepository.Assets.ProjectTo<SelectListItem>().ToList();
        }
    }
}