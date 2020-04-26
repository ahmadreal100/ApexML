using System;
using System.Linq;
using System.Web.Mvc;
using Apex.Core.Entities.AddressE;
using Apex.Service.Abstracts;
using Apex.Web.Controllers.Base;
using Apex.Web.Infrastructure;
using Apex.Web.Models;
using Apex.Web.Models.ViewData;

namespace Apex.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IService<City> _cityService;

        public HomeController(IService<City> cityService)
        {
            _cityService = cityService;
        }

        [IgnorePermission]
        public ActionResult Index()
        {
            var model = new DashboardViewModel();
            return View(model);
        }
        public ActionResult GetCities(long provinceId)
        {
            try
            {
                var cities = _cityService.List.Where(x => x.ProvinceId == provinceId).ToList();
                return JsonGet(new AjaxResult { Status = true, Data = cities });
            }
            catch (Exception e)
            {
                return JsonGet(new AjaxResult { Status = false, Message = e.Message });
            }
        }
    }
}