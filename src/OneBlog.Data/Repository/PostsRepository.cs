using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using OneBlog.Data.Common;
using OneBlog.Data.Contracts;
using OneBlog.Data.Models;
using OneBlog.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Memory;

namespace OneBlog.Data
{
    public class PostsRepository : BaseRepository, IPostsRepository
    {
        private ApplicationDbContext _ctx;
        private JsonService _jsonService;
        private UserManager<ApplicationUser> _userManager;

        public PostsRepository(ApplicationDbContext ctx, IConfigurationRoot config,
            JsonService jsonService,
            UserManager<ApplicationUser> userManager
            )
        {
            _ctx = ctx;
            _jsonService = jsonService;
            _userManager = userManager;
        }

        public Pager<PostItem> Find(int take = 10, int skip = 0)
        {
            var count = _ctx.Posts.Include(m => m.Author).Include(m => m.Comments).Count();
            if (take == 0)  //全部显示
            {
                take = count;
            }

            int currentPage = skip / take + 1;
            int totalItems = count;
            int itemsPerPage = take;
            int pagesLength = ((int)(totalItems / itemsPerPage)) + ((totalItems % itemsPerPage) > 0 ? 1 : 0);
            var posts = new List<PostItem>();

            var list = _ctx.Posts.Include(m => m.Author).Include(m => m.Comments).OrderByDescending(m => m.DatePublished).Skip(skip).Take(take).ToList();

            foreach (var item in list)
            {
                var newItem = _jsonService.GetPost(item);
                posts.Add(newItem);
            }

            var postPager = new Pager<PostItem>(posts);
            postPager.CurrentPage = currentPage;
            postPager.TotalItems = totalItems;
            postPager.ItemsPerPage = take;
            postPager.PagesLength = pagesLength;
            return postPager;
        }

        string GetDescription(string desc, string content)
        {
            if (string.IsNullOrEmpty(desc))
            {
                //var p = WebUtils.StripHtml(content);
                var p = content;
                if (p.Length > 100)
                    return p.Substring(0, 100);

                return p;
            }
            return desc;
        }

        string GetUniqueSlug(string slug)
        {
            string s = slug.Trim();// WebUtils.RemoveIllegalCharacters(slug.Trim());

            // will do for up to 100 unique post titles
            for (int i = 1; i < 101; i++)
            {
                if (IsUniqueSlug(s))
                    break;

                s = string.Format("{0}{1}", slug, i);
            }
            return s;
        }


        bool IsUniqueSlug(string slug)
        {
            return _ctx.Posts.Where(p => p.Slug != null && p.Slug.ToLower() == slug.ToLower())
                .FirstOrDefault() == null ? true : false;
        }
        public static string GetStoryUrl(Posts story)
        {
            return string.Format("{0:0000}/{1:00}/{2:00}/{3}", story.DatePublished.Year, story.DatePublished.Month, story.DatePublished.Day, GetUrlSafeTitle(story));
        }

        public static string GetUrlSafeTitle(Posts story)
        {
            // Characters to replace with underscore
            char[] replacements = @" ""'?*.,+&:;\/#".ToCharArray();

            string[] splits = story.Title.Split(replacements, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder bldr = new StringBuilder();
            foreach (string s in splits)
            {
                bldr.Append(s);
                bldr.Append("-");
            }
            return bldr.ToString(0, bldr.Length - 1);
        }

        /// <summary>
        /// Get single post
        /// </summary>
        /// <param name="id">Post id</param>
        /// <returns>Post object</returns>
        public PostDetail FindById(Guid id)
        {
            try
            {
                var item = _ctx.Posts.Include(m => m.Author).Include(m => m.Comments).FirstOrDefault(m => m.Id == id);
                return _jsonService.GetPostDetail(item);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public PostDetail Update(PostDetail detail)
        {
            var post = _ctx.Posts.FirstOrDefault(m => m.Id == detail.Id);
            if (post == null)
            {
                return null;
            }
            UpdatePostDetail(detail, post);
            _ctx.Posts.Update(post);
            UpdatePostCategories(post, detail.Categories);
            SaveAll();
            return detail;

        }

        public PostDetail Add(PostDetail detail)
        {
            var post = new Posts();
            UpdatePostDetail(detail, post);
            _ctx.Posts.Add(post);
            UpdatePostCategories(post, detail.Categories);
            SaveAll();
            detail.Id = post.Id;
            return detail;
        }

        private void UpdatePostDetail(PostDetail detail, Posts post)
        {
            post.Title = detail.Title;
            //post.Author = string.IsNullOrEmpty(detail.Author) ? Security.CurrentUser.Identity.Name : detail.Author;
            post.Description = GetDescription(detail.Description, detail.Content);
            post.Content = detail.Content;
            post.IsPublished = detail.IsPublished;
            post.HasCommentsEnabled = detail.HasCommentsEnabled;
            post.HasRecommendEnabled = detail.HasRecommendEnabled;
            post.DatePublished = DateTime.ParseExact(detail.DateCreated, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

            // if changing slug, should be unique
            if (post.Slug != detail.Slug)
            {
                post.Slug = GetUniqueSlug(detail.Slug);
            }

            if (string.IsNullOrEmpty(post.Slug))
            {
                post.Slug = GetUrlSafeTitle(post);
            }

            List<string> covers = new List<string>();
            if (!string.IsNullOrEmpty(detail.Cover1))
            {
                covers.Add(detail.Cover1);
            }
            if (!string.IsNullOrEmpty(detail.Cover2))
            {
                covers.Add(detail.Cover2);
            }
            if (!string.IsNullOrEmpty(detail.Cover3))
            {
                covers.Add(detail.Cover3);
            }

            if (covers.Count > 0)
            {
                post.CoverImage = Newtonsoft.Json.JsonConvert.SerializeObject(covers);
            }
            else
            {
                post.CoverImage = null;
            }

            ApplicationUser user = null;

            if (true)
            {
                user = _ctx.Users.FirstOrDefault(m => m.Id == detail.Author.Id);
            }
            else
            {
                user = GetCurrentUserAsync().Result;
            }

            if (user == null)
            {
                throw new Exception("无法查找到用户");
            }

            post.Author = user;
            UpdatePostTags(post, FilterTags(detail.Tags));
        }


        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(AspNetCoreHelper.HttpContext.User);
        }


        List<TagItem> FilterTags(IList<TagItem> tags)
        {
            var uniqueTags = new List<TagItem>();

            if (tags == null)
                return uniqueTags;

            foreach (var t in tags)
            {
                if (!uniqueTags.Any(u => u.TagName == t.TagName))
                {
                    uniqueTags.Add(t);
                }
            }
            return uniqueTags;
        }

        void UpdatePostTags(Posts post, List<TagItem> tags)
        {
            post.Tags = "";
            if (tags == null || tags.Count == 0)
            {
                return;
            }
            var taglist = tags.Select(m => m.TagName).ToList();
            post.Tags = string.Join(",", taglist.ToArray());


            var releation = _ctx.TagsInPosts.Where(m => m.PostId == post.Id).ToList();
            foreach (var item in taglist)
            {
                // add if category does not exist
                var exitTag = _ctx.Tags.FirstOrDefault(m => m.TagName == item);
                if (exitTag != null)
                {
                    var releationTag = releation.FirstOrDefault(m => m.TagId == exitTag.Id);
                    if (releationTag != null)
                    {
                        releation.Remove(releationTag);
                        continue;
                    }
                    _ctx.TagsInPosts.Add(new TagsInPosts() { PostId = post.Id, TagId = exitTag.Id });
                }
                else
                {
                    var tag = new Tags() { TagName = item };
                    _ctx.Tags.Add(tag);
                    _ctx.TagsInPosts.Add(new TagsInPosts() { PostId = post.Id, TagId = tag.Id });
                }
            }

            foreach (var item in releation)
            {
                _ctx.TagsInPosts.Remove(item);
            }
        }

        void UpdatePostCategories(Posts post, IList<CategoryItem> categories)
        {
            var releation = _ctx.PostsInCategories.Where(m => m.PostsId == post.Id).ToList();
            if (categories == null)
            {
                return;
            }
            foreach (var cat in categories)
            {

                // add if category does not exist
                var existingCat = _ctx.Categories.Where(c => c.Title == cat.Title).FirstOrDefault();
                if (existingCat != null)
                {
                    var releationCat = releation.FirstOrDefault(m => m.CategoriesId == existingCat.Id);
                    if (releationCat != null)
                    {
                        releation.Remove(releationCat);
                        continue;
                    }
                    _ctx.PostsInCategories.Add(new PostsInCategories() { PostsId = post.Id, CategoriesId = existingCat.Id });
                }
            }

            foreach (var item in releation)
            {
                _ctx.PostsInCategories.Remove(item);
            }
        }

        public void AddPost(Posts story)
        {
            _ctx.Posts.Add(story);
        }

        public void SaveAll()
        {
            _ctx.SaveChanges();
        }

        public PostsResult GetPosts(int pageSize = 16, int page = 1, Guid? authorId = null)
        {
            var count = 0;

            // Fix random SQL Errors due to bad offset
            if (page < 1) { page = 1; }
            if (pageSize > 100) { pageSize = 100; }
            List<Posts> posts = null;

            if (!authorId.HasValue)
            {
                posts = _ctx.Posts.Include(m => m.Author).Include(m => m.Comments)
                .Where(s => s.IsPublished)
                .OrderByDescending(s => s.DatePublished)
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .ToList();
                count = _ctx.Posts.Where(m => m.IsPublished).Count();
            }
            else
            {
                posts = _ctx.Posts.Include(m => m.Author).Include(m => m.Comments)
                 .Where(s => s.IsPublished && (s.Author.Id == authorId.Value.ToString()))
                 .OrderByDescending(s => s.DatePublished)
                 .Skip(pageSize * (page - 1))
                 .Take(pageSize)
                 .ToList();
                count = _ctx.Posts.Include(m => m.Author).Where(s => s.IsPublished && (s.Author.Id == authorId.Value.ToString())).Count();
            }

            var postlist = new List<PostItem>();

            foreach (var item in posts)
            {
                postlist.Add(_jsonService.GetPost(item));
            }

            List<PostItem> recommendlist = null;

            if (page == 1)
            {
                var temp = _ctx.Posts.Where(m => m.HasRecommendEnabled).OrderByDescending(m => m.DatePublished).Take(3).ToList();
                if (temp != null && temp.Count > 0)
                {
                    recommendlist = new List<PostItem>();
                    foreach (var item in temp)
                    {
                        recommendlist.Add(_jsonService.GetPost(item));
                    }
                }
            }

            var result = new PostsResult()
            {
                CurrentPage = page,
                TotalResults = count,
                TotalPages = CalculatePages(count, pageSize),
                RecommendPosts = recommendlist,
                Posts = postlist,
            };

            return result;
        }

        public PostsResult GetPostsByTerm(string term, int pageSize, int page)
        {
            var lowerTerm = term.ToLowerInvariant();
            var totalCount = _ctx.Posts.Where(s =>
                s.IsPublished &&
                (s.Content.ToLowerInvariant().Contains(lowerTerm) ||
                s.Tags.ToLowerInvariant().Contains(lowerTerm) ||
                s.Title.ToLowerInvariant().Contains(lowerTerm))
                ).Count();

            var posts = _ctx.Posts.Include(m => m.Author).Include(m => m.Comments)
              .Where(s => s.IsPublished && (s.Content.ToLowerInvariant().Contains(lowerTerm) ||
                       s.Tags.ToLowerInvariant().Contains(lowerTerm) ||
                       s.Title.ToLowerInvariant().Contains(lowerTerm)))
              .OrderByDescending(o => o.DatePublished)
              .Skip((page - 1) * pageSize).Take(pageSize);

            var postlist = new List<PostItem>();

            foreach (var item in posts)
            {
                postlist.Add(_jsonService.GetPost(item));
            }


            var result = new PostsResult()
            {
                CurrentPage = page,
                TotalResults = totalCount,
                TotalPages = CalculatePages(totalCount, pageSize),
                Posts = postlist
            };

            return result;

        }


        public Posts GetPost(Guid id)
        {
            var result = _ctx.Posts.Include(m => m.Comments).Where(b => b.Id == id).FirstOrDefault();
            return result;
        }

        public Posts GetPost(string slug)
        {
            var result = _ctx.Posts
              .Where(s => s.Slug == slug || s.Slug == slug.Replace('_', '-'))
              .FirstOrDefault();

            return result;
        }

        public bool DeletePost(Guid postid)
        {
            var id = postid;
            var story = _ctx.Posts.Where(w => w.Id == id).FirstOrDefault();
            if (story != null)
            {
                _ctx.Posts.Remove(story);
                return true;
            }
            return false;
        }

        public IEnumerable<string> GetCategories()
        {
            var cats = _ctx.Posts
                   .Select(c => c.Tags)
                   .ToList();

            var result = new List<string>();
            foreach (var s in cats)
            {
                result.AddRange(s.Split(','));
            }
            return result.Where(s => !string.IsNullOrWhiteSpace(s)).OrderBy(s => s).Distinct();

        }

        public PostsResult GetPostsByTag(string tag, int pageSize, int page)
        {
            var lowerTag = tag.ToLowerInvariant();
            var totalCount = _ctx.Posts
              .Where(s => s.IsPublished && s.Tags.ToLower().Contains(lowerTag)) // Limiting the search for perf
              .ToArray()
              .Where(s => s.Tags.ToLower().Split(',').Contains(lowerTag)).Count();

            var posts = _ctx.Posts.Include(m => m.Author).Include(m => m.Comments)
                .Where(s => s.IsPublished && s.Tags.ToLower().Contains(lowerTag))
                .ToArray()
                .Where(s => s.Tags.ToLower().Split(',').Contains(lowerTag))
                .OrderByDescending(o => o.DatePublished)
                .Skip((page - 1) * pageSize).Take(pageSize);

            var postlist = new List<PostItem>();

            foreach (var item in posts)
            {
                postlist.Add(_jsonService.GetPost(item));
            }


            var result = new PostsResult()
            {
                CurrentPage = page,
                TotalResults = totalCount,
                TotalPages = CalculatePages(totalCount, pageSize),
                Posts = postlist
            };

            return result;
        }

        public PostsResult GetPostsByCategory(Guid categoryId, int pageSize, int page)
        {
            var list = _ctx.PostsInCategories.Include(m => m.Posts).Include(m => m.Posts.Author).Include(m => m.Posts.Comments)
                .Where(m => m.CategoriesId == categoryId).Select(m => m.Posts);

            var totalCount = list.Count();

            var posts = list.ToArray().OrderByDescending(o => o.DatePublished).Skip((page - 1) * pageSize).Take(pageSize);

            var category = _ctx.Categories.Where(m => m.Id == categoryId).FirstOrDefault();

            var postlist = new List<PostItem>();

            foreach (var item in posts)
            {
                postlist.Add(_jsonService.GetPost(item));
            }


            var result = new PostsResult()
            {
                CurrentPage = page,
                TotalResults = totalCount,
                TotalPages = CalculatePages(totalCount, pageSize),
                Posts = postlist,
                Category = category.Title
            };

            return result;
        }

        public string FixContent(string content)
        {
            string Reg = @"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>";
            List<Uri> urlList = new List<Uri>();

            var newContent = content;
            Dictionary<string, string> dict = new Dictionary<string, string>();
            //获取图片URL地址
            var matches = Regex.Matches(content, Reg, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            foreach (Match match in matches)
            {
                var imgUrl = match.Groups["imgUrl"].Value;
                if (!dict.ContainsKey(imgUrl))
                {
                    var newUrl = imgUrl + "?imageView2/0/format/png/q/75|watermark/1/image/aHR0cDovL2Nkbi5kYXRpYW5jdW4uY29tL2RhdGlhbmN1bi5wbmc=/dissolve/80/gravity/SouthEast/dx/10/dy/10|imageslim";
                    dict.Add(imgUrl, newUrl);
                    Uri uri = new Uri(imgUrl);
                    if (uri.Host == "cdn.datiancun.com")
                    {
                        newContent = newContent.Replace("src=\"" + imgUrl, "class=\"lazyload\"  data-src=\"" + newUrl);
                    }
                }
            }
            return newContent;

        }

        public long AddPostCount(Guid id)
        {
            var post = _ctx.Posts.FirstOrDefault(m => m.Id == id);
            if (post != null)
            {
                post.Count += 1;
                _ctx.SaveChanges();
                return post.Count;
            }
            return 1;
        }
    }
}
