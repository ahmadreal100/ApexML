using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Apex.Core.Entities.ShopE;
using Apex.Service.Extensions;
using Apex.Service.Services;
using Apex.Service.ViewModels.Shop;
using Apex.Web.Controllers.Base;
using Apex.Web.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Apex.Web.Controllers.Shop
{
    public class CommentController : BaseController
    {
        private readonly GenericService<Comment, CommentViewModel> _service;

        public CommentController(GenericService<Comment, CommentViewModel> service)
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
            var comments = _service.Repository.Assets.OrderBy(x => x.Seen).ThenByDescending(x => x.Id).ProjectTo<CommentViewModel>();
            return PartialView("_IndexGrid", comments);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                var result = await _service.Delete(id);
                if (result.NotFound)
                    return Json(new AjaxResult("دیدگاه مورد نظر یافت نشد."));

                if (result.Succeeded)
                    return Json(new AjaxResult(true, "دیدگاه مورد نظر با موفقیت حذف شد."));

                return Json(new AjaxResult(result.State.Errors.JoinMessages()));
            }
            catch (Exception e)
            {
                return Json(new AjaxResult(e.JoinMessages()));
            }
        }

        [HttpPost]
        public async Task<ActionResult> Seen(long id, bool seen)
        {
            try
            {
                var item = _service.Repository.Asset(id).ProjectTo<CommentViewModel>().FirstOrDefault();
                if (item == null)
                    return Json(new AjaxResult("دیدگاه مورد نظر یافت نشد."));

                item.Seen = seen;
                var result = await _service.Update(item.Id, Mapper.Map<CommentViewModel>(item));

                if (result.Succeeded)
                    return Json(new AjaxResult(true, "دیدگاه مورد نظر با موفقیت حذف شد."));
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
                var item = _service.Repository.Asset(id).ProjectTo<CommentViewModel>().FirstOrDefault();
                if (item == null)

                    return Json(new AjaxResult("دیدگاه مورد نظر یافت نشد."));

                var viewResult = PartialView("Shop/_ShowComment", item);
                return JsonGet(new AjaxResult(true) { Data = ViewToString(viewResult) });
            }
            catch (Exception e)
            {
                return JsonGet(new AjaxResult(e.JoinMessages()));
            }
        }
    }
}