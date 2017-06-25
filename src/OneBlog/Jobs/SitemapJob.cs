using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OneBlog.Data.Contracts;
using OneBlog.Helpers;
using OneBlog.AspNetCore.TimedJob;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace OneBlog.Jobs
{
    public class SitemapJob : Job
    {
        private IPostsRepository _postsRepository;
        private IHostingEnvironment _env;
        public SitemapJob(IPostsRepository postsRepository, IHostingEnvironment env)
        {
            _postsRepository = postsRepository;
            _env = env;
        }

        [Invoke(Begin = "2017-3-15 00:00", Interval = 1000 * 60 * 5)]
        public void Collect(IServiceProvider services)
        {
            try
            {

                //var accessor = services.GetRequiredService<IActionContextAccessor>();
                //var urlHelperFactory = services.GetRequiredService<IUrlHelperFactory>();
                //var urlHelper = urlHelperFactory.GetUrlHelper(accessor.ActionContext);

                var allPosts = _postsRepository.GetPosts(int.MaxValue);
                XDocument doc = new XDocument(
    new XElement("urlset",
    allPosts.Posts.Select(x => new XElement("url",
    new XElement("loc", "http://chenrensong.com/post/" + x.Id), new XElement("lastmod", x.DatePublished.ToString("yyyy-MM-dd")), new XElement("changefreq", "always"), new XElement("priority", "1"),
    new XElement("data", new XElement("display", new XElement("title", x.Title), new XElement("content", x.Content),
    new XElement("tag", x.Tags.Select(m => m.TagName)), new XElement("pubTime", x.DatePublished.ToString("yyyy-MM-ddThh:mm:ss")),
    new XElement("breadCrumb", new XAttribute("title", "大田村"), new XAttribute("url", "http://chenrensong.com")),
    new XElement("thumbnail", new XAttribute("loc", x.Cover1))))
    )
    )));
                var filename = _env.WebRootPath + $@"\sitemap.xml";
                var directory = Path.GetDirectoryName(filename);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    doc.Save(fs);
                    fs.Flush();
                }
            }
            catch (Exception ex)
            {

            }

        }
    }
}
