using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Apex.Service.Extensions;
using Apex.Service.Services.Shop;
using Apex.Service.ViewModels.Shop;
using Apex.Web.Controllers.Base;
using Apex.Web.Models;

namespace Apex.Web.Controllers.Shop
{
    public class CategoryController : BaseController
    {
        private readonly CategoryService _service;

        public CategoryController(CategoryService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddOrUpdate(CategoryViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new AjaxResult("اطلاعات ورودی صحیح نیست."));

                var isUpdate = model.Id > 0;
                var result = isUpdate ? await _service.Update(model.Id, model) : await _service.Create(model);
                if (result.NotFound)
                    return Json(new AjaxResult("گروه مورد نظر یافت نشد."));

                if (result.Succeeded)
                    return Json(new AjaxResult(true, "اطلاعات با موفقیت ذخیره گردید.", Mapper.Map<CategoryViewModel>(result.Result)) { ExData = isUpdate });

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
                var category = _service.Repository.OneAsset(id, x => x.Parent.Translations, x => x.Translations);
                if (category == null)
                    return JsonGet(new AjaxResult("گروه مورد نظر یافت نشد."));

                var model = Mapper.Map<CategoryViewModel>(category);
                ViewBag.Parent = Mapper.Map<CategoryViewModel>(category.Parent);
                var viewResult = PartialView("Shop/_AddCategory", model);

                return JsonGet(new AjaxResult(true) { Data = ViewToString(viewResult) });
            }
            catch (Exception e)
            {
                return JsonGet(new AjaxResult(e.JoinMessages()));
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                var result = await _service.Delete(id);
                if (result.NotFound)
                    return Json(new AjaxResult("گروه مورد نظر یافت نشد."));


                if (result.Succeeded)
                    return Json(new AjaxResult(true, "گروه مورد نظر با موفقیت حذف شد."));

                return Json(new AjaxResult(result.State.Errors.JoinMessages()));
            }
            catch (Exception e)
            {
                return Json(new AjaxResult(e.JoinMessages()));
            }
        }

        public ActionResult GetChilds(long? id)
        {
            try
            {
                var categories = _service.Repository.Assets.OrderBy(x => x.Id).Where(x => x.ParentId == id)
                    .Include(x => x.Translations)
                    .ProjectTo<CategoryViewModel>().ToList();
                return JsonGet(new AjaxResult(true) { Data = categories });
            }
            catch (Exception e)
            {
                return JsonGet(new AjaxResult(e.JoinMessages()));
            }
        }
    }
}