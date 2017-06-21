using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using One.Data;
using One.Data.Contracts;
using One.Data.Models;
using One.Models.StoreViewModels;
using System;
using System.Collections.Generic;

namespace One.Controllers
{
    [Route("store")]
    public class StoreController : Controller
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IMemoryCache _memoryCache;
        readonly int _pageSize = 60;

        public StoreController(IStoreRepository storeRepository,
             IMemoryCache memoryCache)
        {
            _storeRepository = storeRepository;
            _memoryCache = memoryCache;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return Pager(Guid.Empty, 1);
        }

        [HttpGet("category/{id}")]
        public IActionResult Category(Guid id)
        {
            return Pager(id, 1);
        }

        [HttpGet("page/{page:int?}")]
        public IActionResult Pager(int page)
        {
            return Pager(Guid.Empty, page);
        }

        [HttpGet("category/{id:guid?}/{page:int?}")]
        public IActionResult Pager(Guid id, int page)
        {
            StoreListViewModel vm = new StoreListViewModel();
            var category_cacheKey = $"Store_Pager_Category";
            var app_cacheKey = $"Store_Pager_{id}_{page}";
            string category_cached, app_cached;
            IList<StoreCategories> categories = null;
            StoreAppResult apps = null;
            if (!_memoryCache.TryGetValue(category_cacheKey, out category_cached))
            {
                categories = _storeRepository.GetCategories();
                category_cached = JsonConvert.SerializeObject(categories);
                _memoryCache.Set(category_cacheKey, category_cached, new MemoryCacheEntryOptions() { SlidingExpiration = TimeSpan.FromHours(6) });
            }
            else
            {
                try
                {
                    categories = JsonConvert.DeserializeObject<IList<StoreCategories>>(category_cached);
                }
                catch
                {
                    categories = _storeRepository.GetCategories();
                }
            }
            if (!_memoryCache.TryGetValue(app_cacheKey, out app_cached))
            {
                apps = _storeRepository.GetApps(id, _pageSize, page);
                app_cached = JsonConvert.SerializeObject(categories);
                _memoryCache.Set(category_cacheKey, app_cached, new MemoryCacheEntryOptions() { SlidingExpiration = TimeSpan.FromHours(6) });
            }
            else
            {
                try
                {
                    apps = JsonConvert.DeserializeObject<StoreAppResult>(app_cached);
                }
                catch
                {
                    apps = _storeRepository.GetApps(id, _pageSize, page);
                }
            }
            vm.StoreCategories = categories;
            vm.AppResult = apps;
            return View("Index", vm);
        }



    }
}
