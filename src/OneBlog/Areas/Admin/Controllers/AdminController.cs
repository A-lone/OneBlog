using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using OneBlog.Data.Contracts;
using OneBlog.Data.Models;
using OneBlog.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OneBlog.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Area("Admin")]
    [Route("admin")]
    public class AdminController : Controller
    {

        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IPostsRepository _postsRepository;
        private readonly IHostingEnvironment _env;
        private readonly QiniuService _qiniuService;

        public AdminController(IHostingEnvironment env, QiniuService qiniuService,
            IPostsRepository postsRepository, ICategoriesRepository categoriesRepository)
        {
            _qiniuService = qiniuService;
            _categoriesRepository = categoriesRepository;
            _postsRepository = postsRepository;
            _env = env;
            //HttpContext.User.Identity.Name
            //throw new Exception();
        }


        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("about")]
        public IActionResult About()
        {
            return View();
        }

        [Route("editpost")]
        public IActionResult EditPost()
        {
            return View();
        }

        [Route("lookups")]
        public IActionResult Lookups([FromServices]ILookupsRepository repository)
        {
            return Ok(repository.GetLookups());
        }

        [Route("tags")]
        public IActionResult Tags(int take = 10, int skip = 0, string postId = "", string order = "")
        {
            IEnumerable<TagItem> tags = new List<TagItem>();
            return Ok(tags);
        }

        [HttpGet]
        [Route("categories")]
        public IActionResult Categories(int take = 10, int skip = 0, string filter = "", string order = "")
        {
            var list = _categoriesRepository.Find(take, skip, filter, order);
            return Ok(list);
        }



        [HttpGet]
        [Route("posts")]
        public Pager<PostItem> Posts(int take = 10, int skip = 0, string filter = "", string order = "")
        {
            return _postsRepository.Find(take, skip);
        }


        [HttpPost]
        [Route("posts")]
        public IActionResult Posts([FromBody]PostDetail item)
        {
            var result = _postsRepository.Add(item);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPut]
        [Route("posts/update")]
        public IActionResult PostsUpdate([FromBody]PostDetail item)
        {
            var result = _postsRepository.Update(item);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }


        [HttpPut]
        [Route("posts/delete")]
        public IActionResult PostsDelete([FromBody]List<PostItem> items)
        {
            if (items == null || items.Count == 0)
            {
                return NotFound();
            }

            foreach (var item in items)
            {
                if (item.IsChecked)
                {
                    _postsRepository.DeletePost(item.Id);
                }
            }
            _postsRepository.SaveAll();
            return Ok();
        }

        [HttpGet]
        [Route("posts/{id?}")]
        public IActionResult Posts(Guid id)
        {
            var result = _postsRepository.FindById(id);
            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }


        [Route("customfields")]
        public IActionResult CustomFields()
        {
            return Ok();
        }


        [HttpPost]
        [Route("coverimage")]
        public async Task<IActionResult> CoverImage()
        {
            string url = string.Empty;
            if (Request.Form.ContainsKey("croppedImage"))
            {
                var source = Request.Form["croppedImage"][0];
                string base64 = source.Substring(source.IndexOf(',') + 1);
                base64 = base64.Trim('\0');
                var buffer = Convert.FromBase64String(base64);
                url = await _qiniuService.Upload(buffer);
            }
            else
            {
                if (Request.Form == null || Request.Form.Files == null || Request.Form.Files.Count == 0)
                {
                    NotFound();
                }
                var file = Request.Form.Files[0];

                var filename = ContentDispositionHeaderValue
                .Parse(file.ContentDisposition)
                .FileName
                .Trim();
                url = await _qiniuService.Upload(file);
            }
            return Ok(url);
        }


        [HttpPost]
        [Route("categories")]
        public IActionResult Categories([FromBody]CategoryItem item)
        {
            //var item = Newtonsoft.Json.JsonConvert.DeserializeObject<CategoryItem>(data);
            var result = _categoriesRepository.Add(item);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        //public IActionResult CategoriesDelete(string id)
        //{
        //    Guid gId;
        //    if (Guid.TryParse(id, out gId))
        //        repository.Remove(gId);
        //    return Request.CreateResponse(HttpStatusCode.OK);
        //}


        [HttpPut]
        [Route("categories/processchecked/delete")]
        public IActionResult CategoriesProcessCheckedDelete([FromBody]List<CategoryItem> items)
        {
            if (items == null || items.Count == 0)
            {
                return NotFound();
            }

            foreach (var item in items)
            {
                if (item.IsChecked)
                {
                    _categoriesRepository.Remove(item.Id);
                }
            }
            return Ok();
        }


        //public IActionResult EditPage()
        //{
        //    return View();
        //}


        //public IActionResult FileManager()
        //{
        //    return View();
        //}


        //public IActionResult Resource()
        //{
        //    var lang = BlogSettings.Instance.Culture;
        //    var sb = new StringBuilder();
        //    var cacheKey = "admin.resource.axd - " + lang;
        //    var script = (string)Blog.CurrentInstance.Cache[cacheKey];

        //    if (String.IsNullOrEmpty(script))
        //    {
        //        System.Globalization.CultureInfo culture;
        //        try
        //        {
        //            culture = new System.Globalization.CultureInfo(lang);
        //        }
        //        catch (Exception)
        //        {
        //            culture = OneBlog.Core.WebUtils.GetDefaultCulture();
        //        }

        //        var jc = new BlogCulture(culture, BlogCulture.ResourceType.Admin);

        //        // add SiteVars used to pass server-side values to JavaScript in admin UI
        //        var sbSiteVars = new StringBuilder();

        //        sbSiteVars.Append("ApplicationRelativeWebRoot: '" + OneBlog.Core.WebUtils.ApplicationRelativeWebRoot + "',");
        //        sbSiteVars.Append("RelativeWebRoot: '" + OneBlog.Core.WebUtils.RelativeWebRoot + "',");
        //        sbSiteVars.Append("AbsoluteWebRoot:  '" + OneBlog.Core.WebUtils.AbsoluteWebRoot + "',");

        //        sbSiteVars.Append("IsPrimary: '" + Blog.CurrentInstance.IsPrimary + "',");
        //        sbSiteVars.Append("BlogInstanceId: '" + Blog.CurrentInstance.Id + "',");
        //        sbSiteVars.Append("BlogStorageLocation: '" + Blog.CurrentInstance.StorageLocation + "',");
        //        sbSiteVars.Append("BlogFilesFolder: '" + OneBlog.Core.WebUtils.FilesFolder + "',");

        //        sbSiteVars.Append("GenericPageSize:  '" + BlogConfig.GenericPageSize.ToString() + "',");
        //        sbSiteVars.Append("GalleryFeedUrl:  '" + BlogConfig.GalleryFeedUrl + "',");
        //        sbSiteVars.Append("Version: 'OneBlog.NET " + BlogSettings.Instance.Version() + "'");

        //        sb.Append("SiteVars = {" + sbSiteVars.ToString() + "}; BlogAdmin = { i18n: " + jc.ToJsonString() + "};");
        //        script = sb.ToString();

        //        Blog.CurrentInstance.Cache.Insert(cacheKey, script, null, Cache.NoAbsoluteExpiration, new TimeSpan(3, 0, 0, 0));

        //    }

        //    return JavaScript(script);
        //}



    }
}