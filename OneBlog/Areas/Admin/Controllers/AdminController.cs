using OneBlog.Core;
using OneBlog.Core.Web;
using OneBlog.Core.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Caching;
using System.Web.Mvc;
using System.Linq;
using System.Web.ModelBinding;

namespace OneBlog.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {

        public ActionResult Extensions(string ext)
        {
            Security.DemandUserHasRight(Rights.AccessAdminPages, true);
            ViewBag.ExtensionName = ext;
            return View();
        }

        [HttpPost]
        public ActionResult Extensions(string ext, string settingName, FormCollection collection)
        {
            Security.DemandUserHasRight(Rights.AccessAdminPages, true);
            var Settings = ExtensionManager.GetSettings(ext, settingName);
            foreach (var item in Settings.Parameters)
            {
                item.UpdateScalarValue(collection[item.Name]);
            }
            ViewBag.ExtensionName = ext;
            ViewBag.ErrorMsg = "";
            ViewBag.InfoMsg = Resources.labels.theValuesSaved;
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult EditPage()
        {
            return View();
        }

        public ActionResult EditPost()
        {
            return View();
        }

        public ActionResult FileManager()
        {
            return View();
        }


        public JavaScriptResult Resource()
        {
            var lang = BlogSettings.Instance.Culture;
            var sb = new StringBuilder();
            var cacheKey = "admin.resource.axd - " + lang;
            var script = (string)Blog.CurrentInstance.Cache[cacheKey];

            if (String.IsNullOrEmpty(script))
            {
                System.Globalization.CultureInfo culture;
                try
                {
                    culture = new System.Globalization.CultureInfo(lang);
                }
                catch (Exception)
                {
                    culture = OneBlog.Core.WebUtils.GetDefaultCulture();
                }

                var jc = new BlogCulture(culture, BlogCulture.ResourceType.Admin);

                // add SiteVars used to pass server-side values to JavaScript in admin UI
                var sbSiteVars = new StringBuilder();

                sbSiteVars.Append("ApplicationRelativeWebRoot: '" + OneBlog.Core.WebUtils.ApplicationRelativeWebRoot + "',");
                sbSiteVars.Append("RelativeWebRoot: '" + OneBlog.Core.WebUtils.RelativeWebRoot + "',");
                sbSiteVars.Append("AbsoluteWebRoot:  '" + OneBlog.Core.WebUtils.AbsoluteWebRoot + "',");

                sbSiteVars.Append("IsPrimary: '" + Blog.CurrentInstance.IsPrimary + "',");
                sbSiteVars.Append("BlogInstanceId: '" + Blog.CurrentInstance.Id + "',");
                sbSiteVars.Append("BlogStorageLocation: '" + Blog.CurrentInstance.StorageLocation + "',");
                sbSiteVars.Append("BlogFilesFolder: '" + OneBlog.Core.WebUtils.FilesFolder + "',");

                sbSiteVars.Append("GenericPageSize:  '" + BlogConfig.GenericPageSize.ToString() + "',");
                sbSiteVars.Append("GalleryFeedUrl:  '" + BlogConfig.GalleryFeedUrl + "',");
                sbSiteVars.Append("Version: 'OneBlog.NET " + BlogSettings.Instance.Version() + "'");

                sb.Append("SiteVars = {" + sbSiteVars.ToString() + "}; BlogAdmin = { i18n: " + jc.ToJsonString() + "};");
                script = sb.ToString();

                Blog.CurrentInstance.Cache.Insert(cacheKey, script, null, Cache.NoAbsoluteExpiration, new TimeSpan(3, 0, 0, 0));

            }

            return JavaScript(script);
        }



    }
}