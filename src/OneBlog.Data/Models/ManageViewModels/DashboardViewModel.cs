using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using OneBlog.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace OneBlog.Data.Models.ManageViewModels
{
    public class DashboardViewModel
    {
        private ApplicationDbContext _ctx;
        private JsonService _jsonService;

        /// <summary>
        /// Dashboard vm
        /// </summary>
        public DashboardViewModel(ApplicationDbContext ctx)
        {
            _ctx = ctx;
            _jsonService = new JsonService(ctx);
            _posts = new List<PostItem>();
            _comments = new List<Comments>();
            _trash = new List<TrashItem>();

            LoadProperties();
        }

        #region Properties

        private List<PostItem> _posts;
        private List<Comments> _comments;
        private List<TrashItem> _trash;

        /// <summary>
        /// Draft posts
        /// </summary>
        public List<PostItem> DraftPosts { get; set; }
        /// <summary>
        /// Post published counter
        /// </summary>
        public int PostPublishedCnt { get; set; }
        /// <summary>
        /// Post drafts counter
        /// </summary>
        public int PostDraftCnt { get; set; }
        /// <summary>
        /// Latest comments
        /// </summary>
        public List<CommentItem> Comments
        {
            get
            {
                var comments = new List<CommentItem>();
                var list = _ctx.Comments.Include(m=>m.Author).OrderByDescending(m => m.CommentDate).Take(5).ToList();
                foreach (var c in list)
                {
                    comments.Add(_jsonService.GetComment(c, _comments));
                }
                return comments;
            }
        }
        /// <summary>
        /// Approved comments counter
        /// </summary>
        public int ApprovedCommentsCnt { get; set; }
        /// <summary>
        /// Pending comments counter
        /// </summary>
        public int PendingCommentsCnt { get; set; }
        /// <summary>
        /// Spam comments counter
        /// </summary>
        public int SpamCommentsCnt { get; set; }
        /// <summary>
        /// Draft pages
        /// </summary>
        //public List<PageItem> DraftPages { get; set; }
        /// <summary>
        /// Published pages counter
        /// </summary>
        public int PagePublishedCnt { get; set; }
        /// <summary>
        /// Draft pages counter
        /// </summary>
        public int PageDraftCnt { get; set; }
        /// <summary>
        /// Trash items counter
        /// </summary>
        public List<TrashItem> Trash { get; set; }
        /// <summary>
        /// Log items counter
        /// </summary>
        //public string Logs
        //{
        //    get
        //    {
        //        return GetLogFile();
        //    }
        //}

        #endregion

        #region Private methods

        private void LoadProperties()
        {
            LoadPosts();
            //LoadPages();
            //LoadTrash();
        }

        private void LoadPosts()
        {
            var posts = _ctx.Posts.Include(m => m.Comments).Where(p => p.IsPublished);
            DraftPosts = new List<PostItem>();
            foreach (var p in posts.Where(p => p.IsPublished == false).ToList())
            {
                DraftPosts.Add(_jsonService.GetPost(p));
            }
            PostDraftCnt = DraftPosts == null ? 0 : DraftPosts.Count;
            PostPublishedCnt = posts.Where(p => p.IsPublished).ToList().Count;

            foreach (var p in posts)
            {
                ApprovedCommentsCnt += p.Comments.Count();
                PendingCommentsCnt += p.Comments.Count(c => !c.IsSpam);
                SpamCommentsCnt += p.Comments.Count(c => c.IsSpam);
                _comments.AddRange(p.Comments);
            }
        }

        //private void LoadPages()
        //{
        //    var pages = Page.Pages.Where(p => p.IsVisible);
        //    DraftPages = new List<PageItem>();
        //    foreach (var p in pages.Where(p => p.IsPublished == false && p.IsDeleted == false).ToList())
        //    {
        //        DraftPages.Add(Json.GetPage(p));
        //    }
        //    PageDraftCnt = DraftPages == null ? 0 : DraftPages.Count;
        //    PagePublishedCnt = pages.Where(p => p.IsPublished).ToList().Count;
        //}

        //private void LoadTrash()
        //{
        //    var posts = _ctx.Posts.Include(m => m.Comments).Where(p => !p.IsPublished);
        //    _trash = new List<TrashItem>();
        //    if (posts.Any())
        //    {
        //        foreach (var p in posts)
        //        {
        //            _trash.Add(new TrashItem
        //            {
        //                Id = p.Id,
        //                Title = System.Web.HttpContext.Current.Server.HtmlEncode(p.Title),
        //                RelativeUrl = p.RelativeLink,
        //                ObjectType = "Post",
        //                DateCreated = p.DateCreated.ToString("MM/dd/yyyy HH:mm")
        //            }
        //            );
        //        }
        //    }
        //    var pages = Page.DeletedPages;
        //    if (pages.Any())
        //    {
        //        foreach (var page in pages)
        //        {
        //            _trash.Add(new TrashItem
        //            {
        //                Id = page.Id,
        //                Title = System.Web.HttpContext.Current.Server.HtmlEncode(page.Title),
        //                RelativeUrl = page.RelativeLink,
        //                ObjectType = "Page",
        //                DateCreated = page.DateCreated.ToString("MM/dd/yyyy HH:mm")
        //            }
        //            );
        //        }
        //    }

        //    var comms = new List<Comments>();
        //    foreach (var p in Post.Posts)
        //    {
        //        if (!Security.IsAuthorizedTo(Rights.EditOtherUsersPosts))
        //            if (p.Author.ToLower() != Security.CurrentUser.Identity.Name.ToLower())
        //                continue;

        //        comms.AddRange(p.DeletedComments);
        //    }
        //    if (comms.Any())
        //    {
        //        foreach (var c in comms)
        //        {
        //            _trash.Add(new TrashItem
        //            {
        //                Id = c.Id,
        //                Title = c.Author + ": " + c.Teaser,
        //                RelativeUrl = c.RelativeLink,
        //                ObjectType = "Comment",
        //                DateCreated = c.DateCreated.ToString("MM/dd/yyyy HH:mm")
        //            }
        //            );
        //        }
        //    }
        //    Trash = _trash;
        //}
        //IEnumerable<SelectOption> GetLogs()
        //{
        //    string fileLocation = HostingEnvironment.MapPath(Path.Combine(BlogConfig.StorageLocation, "logger.txt"));
        //    var items = new List<SelectOption>();

        //    if (File.Exists(fileLocation))
        //    {
        //        using (var sw = new StreamReader(fileLocation))
        //        {
        //            string line;
        //            string logItem = "";
        //            int count = 1;
        //            while ((line = sw.ReadLine()) != null)
        //            {
        //                if (line.Contains("*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*"))
        //                {
        //                    // new log item
        //                    if (!string.IsNullOrEmpty(logItem))
        //                    {
        //                        var item = new SelectOption();
        //                        item.OptionName = "Line" + count.ToString();
        //                        item.OptionValue = logItem;
        //                        items.Add(item);
        //                        logItem = "";
        //                        count++;
        //                    }
        //                }
        //                else
        //                {
        //                    // append line to log item
        //                    logItem = logItem + line + "<br/>";
        //                }
        //            }
        //            sw.Close();
        //            return items;
        //        }
        //    }
        //    else
        //    {
        //        return new List<SelectOption>();
        //    }
        //}

        //string GetLogFile()
        //{
        //    string fileLocation = HostingEnvironment.MapPath(Path.Combine(BlogConfig.StorageLocation, "logger.txt"));
        //    var items = new List<SelectOption>();

        //    if (File.Exists(fileLocation))
        //    {
        //        using (var sw = new StreamReader(fileLocation))
        //        {
        //            return sw.ReadToEnd();
        //        }
        //    }
        //    else
        //    {
        //        return string.Empty;
        //    }
        //}

        #endregion
    }
}
