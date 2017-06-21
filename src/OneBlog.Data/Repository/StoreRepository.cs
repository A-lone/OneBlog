using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OneBlog.Data.Contracts;
using OneBlog.Data.Models;

namespace OneBlog.Data
{
    public class StoreRepository : BaseRepository, IStoreRepository
    {
        private readonly ApplicationContext _context;
        public StoreRepository(ApplicationContext context)
        {
            _context = context;
        }
        public IList<StoreCategories> GetCategories()
        {
            var categories = _context.StoreCategories.ToList();
            categories.Insert(0, new StoreCategories() { Id = Guid.Empty, Title = "默认全部" });
            return categories;
        }

        /// <summary>
        /// 获取App列表
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public IList<StoreApp> GetApps(Guid? categoryId = null)
        {
            IList<StoreApp> apps = new List<StoreApp>();
            if (categoryId.HasValue && categoryId.Value != Guid.Empty)
            {
                apps = _context.StoreApp.Include(m => m.Categories).Where(m => m.Categories.Id == categoryId.Value).ToList();
            }
            else
            {
                apps = _context.StoreApp.ToList();
            }
            return apps;
        }


        public StoreAppResult GetApps(Guid? categoryId = null, int pageSize = 60, int page = 1)
        {
            IList<StoreApp> apps = new List<StoreApp>();
            int count = 0;
            string categoryStr = string.Empty;
            if (categoryId.HasValue && categoryId.Value != Guid.Empty)
            {
                count = _context.StoreApp.Include(m => m.Categories).Where(m => m.Categories.Id == categoryId.Value).Count();

                var category = _context.StoreCategories.FirstOrDefault(m => m.Id == categoryId);

                if (category != null)
                {
                    categoryStr = category.Title;
                }

                // Fix random SQL Errors due to bad offset
                if (page < 1) page = 1;
                if (pageSize > 100) pageSize = 100;
                apps = _context.StoreApp.Include(m => m.Categories).Where(m => m.Categories.Id == categoryId.Value)
                    .OrderByDescending(s => s.AppName)
                    .Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .ToList();
            }
            else
            {
                count = _context.StoreApp.Count();
                if (page < 1) page = 1;
                if (pageSize > 100) pageSize = 100;
                apps = _context.StoreApp
                    .OrderByDescending(s => s.AppName)
                    .Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .ToList();

            }
            var result = new StoreAppResult()
            {
                CurrentPage = page,
                TotalResults = count,
                TotalPages = CalculatePages(count, pageSize),
                Apps = apps,
                Category = categoryStr
            };
            return result;
        }

    }
}
