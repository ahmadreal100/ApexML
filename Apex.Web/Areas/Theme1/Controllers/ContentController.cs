using System.Web.Mvc;

namespace Apex.Web.Areas.Theme1.Controllers
{
    public class ContentController : BaseController
    {

        [Route("privacy", Name = "Privacy")]
        public ActionResult Privacy()
        {
            return View();
        }
        //public ActionResult Laws()
        //{
        //    return View();
        //}
    }
}