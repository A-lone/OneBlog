using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OneBlog.Configuration;
using OneBlog.Data.Contracts;
using OneBlog.Data.Models;
using System;

namespace OneBlog.Controllers
{

    [Route("author")]
    public class AuthorController : Controller
    {
        private IPostsRepository _postsRepository;
        private IUsersRepository _usersRepository;
        private IMemoryCache _memoryCache;
        readonly IOptions<AppSettings> _appSettings;

        public AuthorController(IPostsRepository postsRepository, IUsersRepository usersRepository,
            IMemoryCache memoryCache, IOptions<AppSettings> appSettings)
        {
            _usersRepository = usersRepository;
            _postsRepository = postsRepository;
            _memoryCache = memoryCache;
            _appSettings = appSettings;
        }

        [HttpGet("{id}")]
        public IActionResult Index(Guid id)
        {
            return Pager(id, 1);
        }

        [HttpGet("{id}/{page}")]
        public IActionResult Pager(Guid id, int page)
        {
            var cacheKey = $"Author_Index_{id.ToString()}_{page}";
            string cached;
            PostsResult result = null;
            if (!_memoryCache.TryGetValue(cacheKey, out cached))
            {
                result = _postsRepository.GetPosts(_appSettings.Value.PostPerPage, page, id);
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
                    result = _postsRepository.GetPosts(_appSettings.Value.PostPerPage, page, id);
                }
            }
            var userItem = _usersRepository.FindById(id.ToString());

            if (userItem != null && userItem.Profile != null)
            {
                ViewBag.UserProfile = userItem.Profile;
            }
            ViewBag.ControllerName = "Author";
            ViewBag.Id = id.ToString();
            ViewBag.Title = $"{result.Category}";
            return View("_List", result);
        }
    }
}