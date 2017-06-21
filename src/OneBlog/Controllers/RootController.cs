using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OneBlog.Data;
using OneBlog.Data.Contracts;
using OneBlog.Data.Models;
using OneBlog.Helpers;
using OneBlog.Models;
using OneBlog.Models.CommentViewModels;
using OneBlog.RssSyndication;
using OneBlog.Services;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OneBlog.Controllers
{
    [Route("")]
    public class RootController : Controller
    {
        readonly int _pageSize = 15;

        private IMailService _mailService;
        private IPostsRepository _repo;
        private IMemoryCache _memoryCache;
        private ILogger<RootController> _logger;
        private ICommentsRepository _commentsRepository;
        private IViewRenderService _viewRenderService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public RootController(IMailService mailService, UserManager<ApplicationUser> userManager,
                              IPostsRepository repo, ICommentsRepository commentsRepository,
                              IHttpContextAccessor httpContextAccessor,
                              IMemoryCache memoryCache,
                              IViewRenderService viewRenderService, ILogger<RootController> logger)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _viewRenderService = viewRenderService;
            _mailService = mailService;
            _repo = repo;
            _commentsRepository = commentsRepository;
            _memoryCache = memoryCache;
            _logger = logger;
        }


        [ResponseCache(VaryByHeader = "Accept-Encoding", Location = ResponseCacheLocation.Any, Duration = 10)]
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
            if (_memoryCache.TryGetValue(cacheKey, out cached))
            {
                result = JsonConvert.DeserializeObject<PostsResult>(cached);
            }
            if (result == null)
            {
                result = _repo.GetPosts(_pageSize, page);
                if (result != null)
                {
                    cached = JsonConvert.SerializeObject(result);
                    _memoryCache.Set(cacheKey, cached, new MemoryCacheEntryOptions() { SlidingExpiration = TimeSpan.FromMinutes(5) });
                }
            }
            return View("Index", result);
        }

        [HttpGet("captcha")]
        public ActionResult Captcha()
        {
            string code = ValidateHelper.CreateValidateCode(5);
            _session.SetString("ValidateCode", code);
            byte[] bytes = ValidateHelper.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
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
                var count = _repo.AddPostCount(id);
                return Json(new { Count = count });
            }
            catch
            {
                return Json(new { Count = 0 });
            }
        }

        /// <summary>
        /// 文章详情页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("post/{id}")]
        public IActionResult Post(Guid id)
        {
            var cacheKey = $"Root_Post_{id}";
            string cached;
            PostDetail result = null;
            if (_memoryCache.TryGetValue(cacheKey, out cached))
            {
                try
                {
                    result = JsonConvert.DeserializeObject<PostDetail>(cached);
                }
                catch
                {
                    result = null;
                }
            }
            try
            {
                if (result == null)
                {
                    var post = _repo.FindById(id);
                    if (post != null)
                    {
                        post.Content = _repo.FixContent(post.Content);
                        result = post;
                        cached = JsonConvert.SerializeObject(result);
                        _memoryCache.Set(cacheKey, cached, new MemoryCacheEntryOptions() { SlidingExpiration = TimeSpan.FromHours(12) });
                    }
                }
                return View(result);
            }
            catch
            {
                _logger.LogWarning($"Couldn't find the ${id} post");
            }
            return Redirect("/");
        }


        /// <summary>
        /// 关于页面
        /// </summary>
        /// <returns></returns>
        [HttpGet("about")]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet("PrivacyPolicy")]
        public IActionResult PrivacyPolicy()
        {
            return View();
        }

        /// <summary>
        /// 联系我们页面
        /// </summary>
        /// <returns></returns>
        [HttpGet("contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost("contact")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact([FromBody]ContactModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var spamState = VerifyNoSpam(model);
                    if (!spamState.Success)
                    {
                        return BadRequest(new { Reason = spamState.Reason });
                    }

                    await _mailService.SendMail("ContactTemplate.txt", model.Name, model.Email, model.Subject, model.Msg);

                    return Ok(new { Success = true, Message = "Message Sent" });
                }
                else
                {
                    return BadRequest(new { Reason = "Failed to send email..." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to send email from contact page", ex);
                return BadRequest(new { Reason = "Error Occurred" });
            }

        }

        // Brute Force getting rid of my worst emails
        private SpamState VerifyNoSpam(ContactModel model)
        {
            var tests = new string[]
            {
        "improve your seo",
        "improved seo",
        "generate leads",
        "viagra",
        "your team",
        "PHP Developers",
        "working remotely",
        "google search results"
            };

            if (tests.Any(t =>
            {
                return new Regex(t, RegexOptions.IgnoreCase).Match(model.Msg).Success;
            }))
            {
                return new SpamState() { Reason = "Spam Email Detected. Sorry." };
            }
            return new SpamState() { Success = true };
        }


        [HttpGet("Error/{code:int}")]
        public IActionResult Error(int errorCode)
        {
            if (Response.StatusCode == (int)HttpStatusCode.NotFound ||
                errorCode == (int)HttpStatusCode.NotFound ||
                Request.Path.Value.EndsWith("404"))
            {
                return View("NotFound");
            }

            return View();
        }

        [HttpGet("Exception")]
        public IActionResult Exception()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var request = HttpContext.Features.Get<IHttpRequestFeature>();

            if (exception != null && request != null)
            {
                var message = $@"RequestUrl: ${request.Path} Exception: ${exception.Error}";

                ViewBag.Error = message;
                //_mailService.SendMail("logmessage.txt", "Shawn Wildermuth", "shawn@wildermuth.com", "[One Exception]", message);
            }
            return View();
        }

        [HttpGet("feed")]
        public IActionResult Feed()
        {
            var feed = new RssFeed()
            {
                Title = "大田村",
                Description = "大田村、UWP、Windows10、WM10、UWP开源项目",
                Link = new Uri("http://www.datiancun.com/feed"),
                Copyright = "© 2016-2017 datiancun.com"
            };

            var entries = _repo.GetPosts(16);

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

        [HttpGet("calendar")]
        public IActionResult Calendar()
        {
            return View();
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
            var post = _repo.GetPost(model.PostId);
            var commentDetail = new CommentDetail() { PostId = model.PostId, Author = await GetCurrentUserAsync(), Content = model.Content };
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

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

    }
}
