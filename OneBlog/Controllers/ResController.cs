using OneBlog.Core;
using OneBlog.Core.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneBlog.Controllers
{
    public class ResController : Controller
    {
        public ActionResult Image(string picture)
        {
            try
            {
                var fileName = !picture.StartsWith("/") ? string.Format("/{0}", picture) : picture;
                var file = BlogService.GetFile(string.Format("{0}files{1}", Blog.CurrentInstance.StorageLocation, fileName));
                var index = fileName.LastIndexOf(".") + 1;
                var extension = fileName.Substring(index).ToUpperInvariant();
                return new FileStreamResult(new MemoryStream(file.FileContents), string.Compare(extension, "JPG") == 0 ? "image/jpeg" : string.Format("image/{0}", extension));
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult File(string file)
        {
            try
            {
                var fileName = !file.StartsWith("/") ? string.Format("/{0}", file) : file;
                var fileInfo = BlogService.GetFile(string.Format("{0}files{1}", Blog.CurrentInstance.StorageLocation, fileName));
                var index = fileName.LastIndexOf(".") + 1;
                return File(fileInfo.FileContents, fileName.EndsWith(".pdf") ? "application/pdf" : "application/octet-stream", Path.GetFileName(fileName));
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}