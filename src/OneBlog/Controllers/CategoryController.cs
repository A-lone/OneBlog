using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using OneBlog.Data;
using OneBlog.Data.Contracts;
using OneBlog.Data.Models;
using System;

namespace OneBlog.Controllers
{

    [Route("category")]
    public class CategoryController : Controller
    {
        private IPostsRepository _postsRepository;
        private IMemoryCache _memoryCache;
        readonly int _pageSize = 12;

        public CategoryController(IPostsRepository postsRepository, IMemoryCache memoryCache)
        {
            _postsRepository = postsRepository;
            _memoryCache = memoryCache;
        }

        [HttpGet("{id}")]
        public IActionResult Index(Guid id)
        {
            return Pager(id, 1);
        }

        [HttpGet("{id}/{page}")]
        public IActionResult Pager(Guid id, int page)
        {
          
            var cacheKey = $"Categor_Index_{id.ToString()}_{page}";
            string cached;
            PostsResult result = null;
            if (!_memoryCache.TryGetValue(cacheKey, out cached))
            {
                result = _postsRepository.GetPostsByCategory(id, _pageSize, page);
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
                    result = _postsRepository.GetPostsByCategory(id, _pageSize, page);
                }
            }
            ViewBag.ControllerName = "category";
            ViewBag.Id = id.ToString();
            ViewBag.Title = $"{result.Category}";
            return View("_List", result);
        }
    }
}