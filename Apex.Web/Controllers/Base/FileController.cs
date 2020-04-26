using System;
using System.Web;
using System.Web.Mvc;
using Apex.Service.Extensions;
using Apex.Shared.Extensions;
using Apex.Web.Helpers;
using Apex.Web.Models;

namespace Apex.Web.Controllers.Base
{
    public class FileController : BaseController
    {
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            try
            {
                var extension = MimeExtension.GetExtension(file.ContentType);
                var filePath = FileHelper.GetTempPath();
                var mapPath = Server.MapPath(filePath);
                var fullName = FileHelper.CreateSafePath(mapPath, extension, out var name);
                file.SaveAs(fullName);
                return Json(new AjaxResult
                {
                    Status = true,
                    Data = new UploadResult
                    {
                        Path = filePath,
                        Name = name,
                        FullName = $"{filePath}{name}"
                    }
                });
            }
            catch (Exception e)
            {
                return Json(new AjaxResult(e.JoinMessages()));
            }
        }
    }
}