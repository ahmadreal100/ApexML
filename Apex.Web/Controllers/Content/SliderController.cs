using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using Apex.Service.Extensions;
using Apex.Service.Services.Content;
using Apex.Service.ViewModels.Shop;
using Apex.Shared.Helpers;
using Apex.Web.Controllers.Base;
using Apex.Web.Helpers;
using Apex.Web.Models;

namespace Apex.Web.Controllers.Content
{
    public class SliderController : BaseController
    {
        private readonly SliderService _service;

        public SliderController(SliderService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public PartialViewResult IndexGrid()
        {
            // Only grid query values will be available here.
            var sliders = _service.SliderRepository.Assets.ProjectTo<SliderViewModel>();
            return PartialView("_IndexGrid", sliders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddOrUpdate(SliderViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new AjaxResult(ModelState.JoinMessages()));

                var isUpdate = model.Id > 0;

                model.Pictures?.ForEach(x => x.Link = FileHelper.CopyFile(x.Link, AppConst.Folder.Slider));
                if (isUpdate)
                {
                    var pics = _service.SliderRepository.Asset(model.Id).SelectMany(x => x.Pictures).ToList();
                    pics.ForEach(x =>
                    {
                        if (!model.Pictures.Select(a => a.Link).Contains(x.Link))
                            FileHelper.RemoveFile(x.Link);
                    });
                }

                var result = isUpdate ? await _service.Update(model.Id, model) : await _service.Create(model);
                if (result.NotFound)
                    return Json(new AjaxResult("اسلایدر مورد نظر یافت نشد."));

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
                var pictures = _service.SliderRepository.Asset(id).SelectMany(x => x.Pictures).ToList();
                var result = await _service.Delete(id);
                if (result.NotFound)
                    return Json(new AjaxResult("اسلایدر مورد نظر یافت نشد."));

                if (result.Succeeded)
                {
                    pictures.ForEach(x => FileHelper.RemoveFile(x.Link));
                    return Json(new AjaxResult(true, "اسلایدر مورد نظر با موفقیت حذف شد."));
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
                var slider = _service.SliderRepository.Assets.ProjectTo<SliderViewModel>().FirstOrDefault(x => x.Id == id);
                if (slider == null)

                    return JsonGet(new AjaxResult("اسلایدر مورد نظر یافت نشد."));

                var viewResult = PartialView("Content/_AddSlider", slider);
                return JsonGet(new AjaxResult(true) { Data = ViewToString(viewResult) });
            }
            catch (Exception e)
            {
                return JsonGet(new AjaxResult(e.JoinMessages()));
            }
        }
    }
}