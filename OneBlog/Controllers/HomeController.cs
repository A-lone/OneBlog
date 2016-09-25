using OneBlog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneBlog.Models;
using System.Collections;

namespace OneBlog.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {
            ViewBag.Description = this.GetMetaDescription();
            ViewBag.Keywords = this.GetMetaKeywords();
            ViewBag.Title = BlogSettings.Instance.Name;
        }

        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Captcha()
        {
            string code = ValidateUtils.CreateValidateCode(5);
            Session["ValidateCode"] = code;
            byte[] bytes = ValidateUtils.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }

        [HttpGet]
        public ActionResult Comment(Guid id)
        {
            var model = new CommentViewModels();
            OneBlog.Core.Post post = OneBlog.Core.Post.Posts.FirstOrDefault(m => m.Id == id);
            model.PostId = post.Id;
            var comments = post.Comments.Where(m => !m.IsDeleted).ToList();
            // instantiate object
            var nestedComments = new List<Comment>();

            // temporary ID/Comment table
            var commentTable = new Hashtable();

            foreach (var comment in comments)
            {
                // add to hashtable for lookup
                commentTable.Add(comment.Id, comment);

                // check if this is a child comment
                if (comment.ParentId == Guid.Empty)
                {
                    // root comment, so add it to the list
                    nestedComments.Add(comment);
                }
                else
                {
                    // child comment, so find parent
                    var parentComment = commentTable[comment.ParentId] as Comment;
                    if (parentComment != null)
                    {
                        // double check that this sub comment has not already been added
                        if (parentComment.Comments.IndexOf(comment) == -1)
                        {
                            parentComment.Comments.Add(comment);
                        }
                    }
                    else
                    {
                        // just add to the base to prevent an error
                        nestedComments.Add(comment);
                    }
                }
            }
            model.Comments = nestedComments.OrderBy(m => m.DateCreated).ToList();
            return PartialView(model);
        }

        /// <summary>
        /// 评论
        /// </summary>
        /// <param name="model"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Comment(CommentViewModels model, FormCollection collection)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    Error = "提交的信息有误,请检查后再试"
                });
            }
            if (Session["ValidateCode"].ToString() != model.Captcha)
            {
                return Json(new
                {
                    Error = "提交的验证码错误！",
                });
            }
            var replyToCommentId = collection["hiddenReplyTo"].ToString();
            OneBlog.Core.Post post = OneBlog.Core.Post.Posts.FirstOrDefault(m => m.Id == model.PostId);
            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                ParentId = String.IsNullOrEmpty(replyToCommentId) ? Guid.Empty : new Guid(replyToCommentId),
                Author = HttpUtility.HtmlAttributeEncode(model.UserName),
                Email = Server.HtmlEncode(model.Email),
                Content = HttpUtility.HtmlAttributeEncode(model.Content),
                IP = WebUtils.GetClientIP(),
                //Country = Server.HtmlEncode(country),
                DateCreated = DateTime.Now,
                Parent = post,
                IsApproved = !BlogSettings.Instance.EnableCommentsModeration,
                //Avatar = Server.HtmlEncode(avatar.Trim())
            };
            post.AddComment(comment);
            return Json(new { Error = "", CommentId = comment.Id, CommentCount = (post.Comments.Count + 1), Result = this.RenderViewToString("_Comment", comment), Content = model.Content }, JsonRequestBehavior.AllowGet);
        }


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

        /// <summary>
        /// 分类页
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
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

            ViewBag.Description = this.GetMetaDescription(string.IsNullOrWhiteSpace(cat.Description) ? cat.Title : cat.Description);
            return View("Index", model);
        }


        /// <summary>
        /// 菜单页
        /// </summary>
        /// <param name="p"></param>
        /// <param name="category"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 文章页面
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public ActionResult Post(string slug, int year = 0, int month = 0, int day = 0)
        {
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
                return RedirectToAction("Index", "Home");
            }

            PostViewModels model = new Models.PostViewModels();
            model.Post = post;

            ViewBag.Title = post.Title;

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