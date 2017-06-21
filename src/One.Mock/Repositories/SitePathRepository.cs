using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using One.Mock.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace One.Mock.Repositories
{
    public class SitePathRepository : ISitePathRepository
    {
        private readonly ApplicationContext _context;

        private readonly ILogger _logger;

        public SitePathRepository(ApplicationContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<ISiteRepository>();
        }

        public void Delete(Guid id)
        {
            var entity = _context.SitePaths.First(t => t.Id == id);
            _context.SitePaths.Remove(entity);
            _context.SaveChanges();
        }

        public List<SitePath> Get(Guid id)
        {
            var site = _context.Sites.Include(m => m.SitePaths).FirstOrDefault(m => m.Id == id);
            return site.SitePaths.ToList();
        }

        public List<SitePath> GetAll()
        {
            return _context.SitePaths.ToList();
        }

        public void Post(SitePath site)
        {
            _context.SitePaths.Add(site);
            _context.SaveChanges();
        }

        public void Put(Guid id, [FromBody] SitePath site)
        {
            _context.SitePaths.Update(site);
            _context.SaveChanges();
        }
    }
}