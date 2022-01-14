using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DACN.Controllers
{
    public class UploadController : Controller
    {
        [HttpGet]
        public ActionResult SaveImage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveImage(HttpPostedFileBase uploadedImage)
        {
            if (uploadedImage.ContentLength > 0)
            {
                string ImageFileName = Path.GetFileName(uploadedImage.FileName);

                string FolderPath = Path.Combine(Server.MapPath("~/Upload/Images"), ImageFileName);

                uploadedImage.SaveAs(FolderPath);
            }

            ViewBag.Message = "Image file uploaded successfully";

            return View();
        }
    }
}