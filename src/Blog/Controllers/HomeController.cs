using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using One.Data;
using One.Data.Contracts;
using One.Data.Models;
using One.Helpers;
using One.Models.CommentViewModels;
using One.RssSyndication;
using One.Services;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {

        private readonly IPostsRepository _postsRepository;
        private IMemoryCache _memoryCache;
        private readonly Guid AuthorId = new System.Guid("de3d4cee-3bc4-465e-8b62-8038fed682b8");
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private ICommentsRepository _commentsRepository;
        private IViewRenderService _viewRenderService;

        public HomeController(IPostsRepository postsRepository, ICommentsRepository commentsRepository, IViewRenderService viewRenderService,
            IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache)
        {
            _commentsRepository = commentsRepository;
            _postsRepository = postsRepository;
            _viewRenderService = viewRenderService;
            _httpContextAccessor = httpContextAccessor;
            _memoryCache = memoryCache;
        }


        [HttpGet("captcha")]
        public ActionResult Captcha()
        {
            string code = ValidateHelper.CreateValidateCode(5);
            _session.SetString("ValidateCode", code);
            byte[] bytes = ValidateHelper.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }

        [HttpGet("")]
        [HttpPost("")]
        public IActionResult Index()
        {
            return Pager(1);
        }


        [HttpGet("page/{page:int?}")]
        public IActionResult Pager(int page)
        {
            var cacheKey = $"Root_Pager_{page}";
            string cached;
            PostsResult result = null;
            if (!_memoryCache.TryGetValue(cacheKey, out cached))
            {
                result = _postsRepository.GetPosts(12, page, AuthorId);
                if (result != null)
                {
                    cached = JsonConvert.SerializeObject(result);
                    _memoryCache.Set(cacheKey, cached, new MemoryCacheEntryOptions() { SlidingExpiration = TimeSpan.FromMinutes(5) });
                }
            }
            else
            {
                try
                {
                    result = JsonConvert.DeserializeObject<PostsResult>(cached);
                }
                catch
                {
                    result = _postsRepository.GetPosts(12, page, AuthorId);
                }
            }
            return View("Index", result);
        }


        /// <summary>
        /// 文章详情页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("post/{id}")]
        public IActionResult Post(Guid id)
        {
            try
            {
                var post = _postsRepository.FindById(id);
                if (post != null)
                {
                    post.Content = _postsRepository.FixContent(post.Content);
                    return View(post);
                }
            }
            catch
            {
            }
            return Redirect("/");
        }

        /// <summary>
        /// 统计接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("postcount/{id?}")]
        public IActionResult PostCount(Guid id)
        {
            try
            {
                var count = _postsRepository.AddPostCount(id);
                return Json(new { Count = count });
            }
            catch
            {
                return Json(new { Count = 0 });
            }
        }


        [HttpGet("feed")]
        public IActionResult Feed()
        {
            var feed = new RssFeed()
            {
                Title = "陈仁松同学",
                Description = "陈仁松同学 - stay hungry stay foolish",
                Link = new Uri("http://chenrensong.com/feed"),
                Copyright = "© 2015-2017 chenrensong.com"
            };

            var entries = _postsRepository.GetPosts(100, 1, AuthorId);

            foreach (var entry in entries.Posts)
            {
                var item = new RssItem()
                {
                    Title = entry.Title,
                    Body = string.Concat(entry.Content),
                    Link = new Uri(new Uri(Request.GetEncodedUrl()), "post/" + entry.Id.ToString()),
                    Permalink = entry.Slug,
                    PublishDate = entry.DatePublished,
                    Author = new RssAuthor() { Name = entry.Author.Name, Email = entry.Author.Email }
                };

                foreach (var cat in entry.Tags)
                {
                    item.Categories.Add(cat.TagName);
                }
                feed.Items.Add(item);
            }

            return File(Encoding.UTF8.GetBytes(feed.Serialize()), "text/xml");

        }


        /// <summary>
        /// 评论
        /// </summary>
        /// <param name="model"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost("comment")]
        public async Task<IActionResult> Comment(CommentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    Error = "提交的信息有误,请检查后再试"
                });
            }
            var validateCode = _session.GetString("ValidateCode");
            if (string.IsNullOrEmpty(validateCode))
            {
                return Json(new
                {
                    Error = "验证码过期，请刷新重试！",
                });
            }
            _session.Remove("ValidateCode");
            if (!string.Equals(validateCode, model.Captcha, StringComparison.OrdinalIgnoreCase))
            {
                return Json(new
                {
                    Error = "提交的验证码错误！",
                });
            }
            var replyToCommentId = Request.Form["hiddenReplyTo"].ToString();
            var post = _postsRepository.GetPost(model.PostId);
            ApplicationUser user = new ApplicationUser();
            user.Id = string.Empty;
            user.UserName = model.UserName;
            user.DisplayName = model.UserName;
            user.Email = model.Email;
            var commentDetail = new CommentDetail() { PostId = model.PostId, Author = user, Content = model.Content };
            Guid parentId;
            if (!string.IsNullOrEmpty(replyToCommentId) && Guid.TryParse(replyToCommentId, out parentId))
            {
                commentDetail.ParentId = parentId;
            }
            var comment = _commentsRepository.Add(commentDetail);
            var result = await _viewRenderService.RenderToStringAsync(this, "_Comment", comment);
            return Json(new
            {
                Error = "",
                CommentId = comment.Id,
                CommentCount = (post.Comments.Count + 1),
                Result = result,
                Content = model.Content
            });
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
