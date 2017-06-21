using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using One.Mock.Data;
using One.Mock.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace One.Mock.Repositories
{
    public class SiteRepository : ISiteRepository
    {
        private readonly ApplicationContext _context;

        private readonly ILogger _logger;

        public SiteRepository(ApplicationContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<ISiteRepository>();
        }

        public void Delete(Guid id)
        {
            var entity = _context.Sites.First(t => t.Id == id);
            _context.Sites.Remove(entity);
            _context.SaveChanges();
        }

        public Site GetDefault()
        {
            return _context.Sites.Include(m=>m.SitePaths).FirstOrDefault(t => t.IsDefault);
        }

        public Site Get(Guid id)
        {
            return _context.Sites.First(t => t.Id == id);
        }

        public List<Site> GetAll()
        {
            return _context.Sites.ToList();
        }

        public void Post(SiteVM siteVM)
        {
            var site = new Site();
            site.Cookie = siteVM.Cookie;
            site.IsDefault = siteVM.IsDefault;
            site.Name = siteVM.Name;
            site.Url = siteVM.Url;
            _context.Sites.Add(site);
            _context.SaveChanges();
        }

        public void Put(Guid id, [FromBody] SiteVM siteVM)
        {
            var site = _context.Sites.FirstOrDefault(m => m.Id == siteVM.Id);
            if (site != null)
            {
                site.Cookie = siteVM.Cookie;
                site.IsDefault = siteVM.IsDefault;
                site.Name = siteVM.Name;
                site.Url = siteVM.Url;
                _context.Sites.Update(site);
                _context.SaveChanges();
            }
        }
    }
}