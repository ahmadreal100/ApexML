using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Apex.Service.Services.Shop;

namespace Apex.Web.Areas.Theme1.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly CategoryService _service;

        public CategoryController(CategoryService service)
        {
            _service = service;
        }
        public async Task<ActionResult> Get()
        {
            try
            {
                var categories = await _service.GetAllNested();
                return JsonGet(categories);
            }
            catch (Exception)
            {
                return JsonGet(new List<object>());
            }
        }

    }
}