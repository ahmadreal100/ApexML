using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Apex.Core.Entities.ShopE;
using Apex.Service.Abstracts;
using Apex.Service.Extensions;
using Apex.Service.Services;
using Apex.Service.Services.Content;
using Apex.Service.Services.Shop;
using Apex.Service.Translations;
using Apex.Service.ViewModels.Shop;
using Apex.Web.Areas.Theme1.Models.DataViewModels;
using Apex.Web.Infrastructure;
using Apex.Web.Models;
using AutoMapper.QueryableExtensions;

namespace Apex.Web.Areas.Theme1.Controllers
{
    public class HomeController : BaseController
    {
        private readonly SliderService _sliderService;
        private readonly ProductService _productService;
        private readonly IUserService _userService;
        private readonly GenericService<Comment, CommentDto> _commentService;
        public HomeController(SliderService sliderService, ProductService productService, IUserService userService, MessageService messageService, GenericService<Comment, CommentDto> commentService)
        {
            _sliderService = sliderService;
            _productService = productService;
            _userService = userService;
            _commentService = commentService;
        }
        // GET: Home
        [Route("", Name = "Home")]
        public async Task<ActionResult> Index()
        {
            //await _messageService.SendOperatorPassAsync("+989178574885", "احمد روح بخش بهحانی", "e6GxN0", "1397/12/03 13:45");

            var model = new IndexViewData
            {
                Slider0 = _sliderService.SliderRepository.Queryable().Where(x => x.Location == 0)
                              .ProjectTo<SliderViewModel>()
                              .FirstOrDefault() ?? new SliderViewModel { Pictures = new List<SliderPictureViewModel>() },
                LastProducts = await (await _productService.GetThumbs()).Take(12).ToListAsync()
            };


            return View(model);
        }

        [Route("about", Name = "About")]
        public ActionResult About()
        {
            return View();
        }

        [Route("contact", Name = "Contact")]
        public ActionResult Contact()
        {
            return View();
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [ArCaptcha]
        public async Task<ActionResult> AddShopComment(CommentDto model)
        {
            try
            {
                model.UserId = _userService.RequestInfo.UserId;
                if (!ModelState.IsValid)
                    return Json(new AjaxResult(ModelState.JoinMessages()));

                var result = await _commentService.Create(model);

                if (result.Succeeded)
                    return Json(new AjaxResult(true, Str.operationSuccessful));
                return Json(new AjaxResult(Str.operationFailed));
            }
            catch
            {
                return Json(new AjaxResult(Str.operationFailed));
            }
        }


        //[Route("Visits/Image", Title = "VisitImage")]
        //public async Task<ActionResult> Visit(VisitViewModel model)
        //{
        //    try
        //    {
        //        await _visitService.Create(model);
        //        Response.ContentType = "image/gif";
        //        return ViewHelper.GetTinyGif();
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new AjaxResult { Status = false, Data = e.Message });
        //    }
        //}

        //public ActionResult Error()
        //{
        //    if (Request.IsAjaxRequest())
        //        return HttpNotFound();

        //    var code = Regex.Match(HttpContext?.Request.Url?.PathAndQuery ?? "", @"(?<=\?)\d{3}(?=;?)").Value;
        //    ViewBag.ErrorCode = code.IsInteger(0);
        //    var model = new ContactViewModel();
        //    SetLayoutViewModel(model);
        //    return View(model);
        //}


        [Route("captcha", Name = "Recaptcha")]
        public ActionResult GenerateArCaptcha()
        {
            var arCaptcha = new ArCaptcha(new ArCaptchaSetting
            {
                Type = ArCaptchaType.Digit,
                MinNumber = 101,
                MaxNumber = 999999,
                Font = new Font(new FontFamily("Tahoma"), 15f, FontStyle.Bold),
                FontColor = (Color)(new ColorConverter().ConvertFromString("#3a7065") ?? Color.DarkSlateGray),
                Width = 100,
                Height = 35
            });

            var image = arCaptcha.Generate(out _);
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg);
                return File(ms.ToArray(), "image/jpeg");
            }
        }
    }
}