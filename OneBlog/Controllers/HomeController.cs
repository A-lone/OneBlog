using OneBlog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneBlog.Models;

namespace OneBlog.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Rss()
        {
            return Redirect("/rss.axd");
        }

        public ActionResult Index()
        {
            var posts = OneBlog.Core.Post.ApplicablePosts.ConvertAll(new Converter<Post, IPublishable>(delegate (Post p) { return p as IPublishable; }));
            HomeViewModels model = new HomeViewModels();
            model.Posts = posts;
            var first = posts.FirstOrDefault();
            model.CoverPost = first == null ? new Post() : first;
            return View(model);
        }


        public ActionResult Category(string category)
        {

            var cat = (from item in OneBlog.Core.Category.ApplicableCategories
                                  let legalTitle = WebUtils.RemoveIllegalCharacters(item.Title).ToLowerInvariant()
                                  where category.Equals(legalTitle, StringComparison.OrdinalIgnoreCase)
                                  select item).FirstOrDefault();

            var posts = OneBlog.Core.Post.GetPostsByCategory(cat).ConvertAll(new Converter<Post, IPublishable>(delegate (Post p) { return p as IPublishable; }));
            HomeViewModels model = new HomeViewModels();
            model.Posts = posts;
            var first = posts.FirstOrDefault();
            model.CoverPost = first == null ? new Post() : first;
            return View("Index", model);
        }


        public PartialViewResult Menu(int? p, string category)
        {
            MenuViewModels model = new Models.MenuViewModels();
            var pageIndex = p ?? 1;
            var pageSize = BlogSettings.Instance.PostsPerPage;
            List<IPublishable> list = null;

            if (string.IsNullOrEmpty(category))
            {
                list = OneBlog.Core.Post.ApplicablePosts.ConvertAll(new Converter<Post, IPublishable>(delegate (Post post) { return post as IPublishable; }));
            }
            else
            {
                var cat = (from item in OneBlog.Core.Category.ApplicableCategories
                           let legalTitle = WebUtils.RemoveIllegalCharacters(item.Title).ToLowerInvariant()
                           where category.Equals(legalTitle, StringComparison.OrdinalIgnoreCase)
                           select item).FirstOrDefault();
                list = OneBlog.Core.Post.GetPostsByCategory(cat).ConvertAll(new Converter<Post, IPublishable>(delegate (Post post) { return post as IPublishable; }));
            }

            var totalCount = list.Count;
            var results = list
                 .OrderByDescending(x => x.DateCreated)
                 .Skip((pageIndex - 1) * pageSize)
                 .Take(pageSize)
                 .ToList();

            // Return a paged list
            var pageList = new PagedList<IPublishable>(results, pageIndex, pageSize, totalCount);

            model.Posts = pageList;
            model.PageIndex = pageList.PageIndex;
            model.TotalPages = pageList.TotalPages;
            model.TotalCount = pageList.TotalCount;
            return PartialView("_Menu", model);
        }

        public ActionResult Post(string slug)
        {
            int year = 0, month = 0, day = 0;
            var haveDate = false;
            // Allow for Year/Month only dates in URL (in this case, day == 0), as well as Year/Month/Day dates.
            // first make sure the Year and Month match.
            // if a day is also available, make sure the Day matches.
            var post = OneBlog.Core.Post.ApplicablePosts.Find(
                p =>
                (!haveDate || (p.DateCreated.Year == year && p.DateCreated.Month == month)) &&
                ((!haveDate || (day == 0 || p.DateCreated.Day == day)) &&
                 slug.Equals(WebUtils.RemoveIllegalCharacters(p.Slug), StringComparison.OrdinalIgnoreCase)));

            if (post == null)
            {
                return View();
            }

            PostViewModels model = new Models.PostViewModels();
            model.Post = post;
            return View(model);
        }


        //public ActionResult Archive()
        //{
        //    ArchiveViewModels model = new ArchiveViewModels();
        //    model.Category = OneBlog.Core.Category.ApplicableCategories.Where(m => m.Posts.Count > 0).ToList();
        //    List<Post> posts = OneBlog.Core.Post.ApplicablePosts.FindAll(delegate (Post p) { return p.IsVisible; });
        //    foreach (Post post in posts)
        //    {
        //        model.CommentsCount += post.ApprovedComments.Count;
        //        model.RatersCount += post.Raters;
        //    }
        //    model.PostsCount = posts.Count;
        //    return View(model);
        //}


        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";
        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ContactViewModels model = new Models.ContactViewModels();
        //    return View(model);
        //}

        //[HttpPost]
        //public ActionResult Contact(ContactViewModels model)
        //{
        //    return View(model);
        //}
    }
}