using OneBlog.Data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OneBlog.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace OneBlog.Data.Common
{
    /// <summary>
    /// Helper class for Json operation
    /// </summary>
    public class JsonService
    {
        private readonly ApplicationContext _ctx;

        public JsonService(ApplicationContext ctx)
        {
            _ctx = ctx;
        }

        /// <summary>
        /// Get post converted to Json
        /// </summary>
        /// <param name="post">Post</param>
        /// <returns>Json post</returns>
        public PostItem GetPost(Posts post)
        {
            var categories = _ctx.PostsInCategories.Where(m => m.PostsId == post.Id).Select(m => m.Categories).ToList();

            var author = new Author();
            author.Id = post.Author?.Id;
            author.Signature = post.Author?.Signature;
            author.DisplayName = post.Author?.DisplayName;
            author.Name = post.Author?.UserName;
            var postitem = new PostItem
            {
                Id = post.Id,
                Author = author,
                Title = post.Title,
                Content = post.Content,
                Slug = post.Slug,
                RelativeLink = "/post/" + post.Id,
                CommentsCount = post.Comments != null ? post.Comments.Count : 0,
                ReadCount = post.Count,
                DatePublished = post.DatePublished,
                DateCreated = post.DatePublished.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                Categories = GetCategories(categories),
                Tags = GetTags(post.Tags.Split(',')),
                Comments = GetComments(post),
                IsPublished = post.IsPublished,
            };
            if (!string.IsNullOrEmpty(post.CoverImage))
            {
                var covers = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(post.CoverImage);
                if (covers.Count > 0)
                {
                    postitem.Cover1 = covers[0];
                }
                if (covers.Count > 1)
                {
                    postitem.Cover2 = covers[1];
                }
                if (covers.Count > 2)
                {
                    postitem.Cover2 = covers[2];
                }
            }
            return postitem;
        }
        /// <summary>
        /// Get detailed post
        /// </summary>
        /// <param name="post">Post</param>
        /// <returns>Json post detailed</returns>
        public PostDetail GetPostDetail(Posts post)
        {
            var categories = _ctx.PostsInCategories.Where(m => m.PostsId == post.Id).Select(m => m.Categories).ToList();

            var author = new Author();
            author.Id = post.Author?.Id;
            author.Signature = post.Author?.Signature;
            author.DisplayName = post.Author?.DisplayName;
            author.Name = post.Author?.UserName;
            author.Avatar = post.Author?.Avatar;
            author.SiteUrl = post.Author?.SiteUrl;
            var postDetail = new PostDetail
            {
                Id = post.Id,
                Author = author,
                Title = post.Title,
                Slug = post.Slug,
                Description = post.Description,
                RelativeLink = "/post/" + post.Id,
                Content = post.Content,
                DateCreated = post.DatePublished.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                Categories = GetCategories(categories),
                Tags = GetTags(post.Tags.Split(',')),
                Comments = GetComments(post),
                HasCommentsEnabled = post.HasCommentsEnabled,
                HasRecommendEnabled = post.HasRecommendEnabled,
                IsPublished = post.IsPublished,
                IsDeleted = false,
                CanUserEdit = true,
                CanUserDelete = true
            };
            if (!string.IsNullOrEmpty(post.CoverImage))
            {
                var covers = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(post.CoverImage);
                if (covers.Count > 0)
                {
                    postDetail.Cover1 = covers[0];
                }
                if (covers.Count > 1)
                {
                    postDetail.Cover2 = covers[1];
                }
                if (covers.Count > 2)
                {
                    postDetail.Cover2 = covers[2];
                }
            }
            return postDetail;
        }

        ///// <summary>
        ///// Get page converted to json
        ///// </summary>
        ///// <param name="page">Page</param>
        ///// <returns>Json page</returns>
        //public static PageItem GetPage(Page page)
        //{
        //    Page parent = null;
        //    SelectOption parentOption = null;

        //    if (page.Parent != Guid.Empty)
        //    {
        //        parent = Page.Pages.FirstOrDefault(p => p.Id.Equals(page.Parent));
        //        parentOption = new SelectOption { IsSelected = false, OptionName = parent.Title, OptionValue = parent.Id.ToString() };
        //    }
        //    return new PageItem
        //    {
        //        Id = page.Id,
        //        ShowInList = page.ShowInList,
        //        Title = page.Title,
        //        Slug = page.Slug,
        //        Parent = parentOption,
        //        Keywords = page.Keywords,
        //        DateCreated = page.DateCreated.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
        //        HasChildren = page.HasChildPages,
        //        IsPublished = page.IsPublished,
        //        IsFrontPage = page.IsFrontPage,
        //        SortOrder = page.SortOrder,
        //    };
        //}
        ///// <summary>
        ///// Get page details
        ///// </summary>
        ///// <param name="page">Page</param>
        ///// <returns>Json page details</returns>
        //public static PageDetail GetPageDetail(Page page)
        //{
        //    Page parent = null;
        //    SelectOption parentOption = null;

        //    if (page.Parent != Guid.Empty)
        //    {
        //        parent = Page.Pages.FirstOrDefault(p => p.Id.Equals(page.Parent));
        //        parentOption = new SelectOption { IsSelected = false, OptionName = parent.Title, OptionValue = parent.Id.ToString() };
        //    }
        //    return new PageDetail
        //    {
        //        Id = page.Id,
        //        ShowInList = page.ShowInList,
        //        Title = page.Title,
        //        Slug = page.Slug,
        //        RelativeLink = page.RelativeLink,
        //        Content = page.Content,
        //        Parent = parentOption,
        //        Description = page.Description,
        //        Keywords = page.Keywords,
        //        DateCreated = page.DateCreated.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
        //        HasChildren = page.HasChildPages,
        //        IsPublished = page.IsPublished,
        //        IsFrontPage = page.IsFrontPage,
        //        IsDeleted = page.IsDeleted,
        //        SortOrder = page.SortOrder,
        //    };
        //}
        /// <summary>
        /// Get json comment
        /// </summary>
        /// <param name="c">Comment</param>
        /// <param name="postComments">List of comments</param>
        /// <returns>Json comment</returns>
        public CommentItem GetComment(Comments c, List<Comments> postComments)
        {
            var jc = new CommentItem();
            jc.Id = c.Id;
            jc.IsApproved = c.IsApproved;
            jc.IsSpam = c.IsSpam;
            jc.IsPending = !c.IsApproved && !c.IsSpam;
            jc.Author = new Author()
            {
                DisplayName = c.Author.DisplayName,
                Email = c.Author.Email,
                Id = c.Author.Id,
                Name = c.Author.UserName,
                Signature = c.Author.Signature,
                SiteUrl = c.Author.SiteUrl,
                Avatar = c.Author.Avatar
            };
            jc.Content = c.Content;
            jc.Title = c.Content.Length < 80 ? c.Content : c.Content.Substring(0, 80) + "...";
            jc.DateCreated = c.CommentDate.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            jc.HasChildren = postComments.Where(pc => pc.ParentId == c.Id).FirstOrDefault() != null;
            return jc;
        }



        /// <summary>
        ///     Get comment detail
        /// </summary>
        /// <param name="c">Comment</param>
        /// <returns>Json comment detail</returns>
        public CommentDetail GetCommentDetail(Comments c)
        {
            var jc = new CommentDetail();
            jc.Id = c.Id;
            jc.ParentId = c.ParentId;
            jc.PostId = c.Posts.Id;
            jc.Content = c.Content;
            jc.Ip = c.Ip;
            return jc;
        }

        ///// <summary>
        ///// Convert json comment back to BE comment
        ///// </summary>
        ///// <param name="c">Json comment</param>
        ///// <returns>Comment</returns>
        //public static Comment SetComment(CommentItem c)
        //{
        //    Comment item = (from p in Post.Posts
        //                    from cmn in p.AllComments
        //                    where cmn.Id == c.Id
        //                    select cmn).FirstOrDefault();

        //    if (c.IsPending)
        //    {
        //        item.IsApproved = false;
        //        item.IsSpam = false;
        //    }
        //    if (c.IsApproved)
        //    {
        //        item.IsApproved = true;
        //        item.IsSpam = false;
        //    }
        //    if (c.IsSpam)
        //    {
        //        item.IsApproved = false;
        //        item.IsSpam = true;
        //    }

        //    item.Email = c.Email;
        //    item.Author = c.Author;
        //    return item;
        //}

        //#region Private methods

        public List<CategoryItem> GetCategories(ICollection<Categories> categories)
        {
            if (categories == null || categories.Count == 0)
            {
                return null;
            }

            //var html = categories.Aggregate("", (current, cat) => current + string.Format
            //("<a href='#' onclick=\"ChangePostFilter('Category','{0}','{1}')\">{1}</a>, ", cat.Id, cat.Title));
            var categoryList = new List<CategoryItem>();
            foreach (var coreCategory in categories)
            {
                var item = new CategoryItem();
                item.Id = coreCategory.Id;
                item.Title = coreCategory.Title;
                item.Description = coreCategory.Description;
                item.Parent = ItemParent(coreCategory.ParentId);
                categoryList.Add(item);
            }
            return categoryList;
        }

        SelectOption ItemParent(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                return null;

            var item = _ctx.Categories.Where(c => c.Id == id).FirstOrDefault();
            return new SelectOption { OptionName = item.Title, OptionValue = item.Id.ToString() };
        }

        List<TagItem> GetTags(ICollection<string> tags)
        {
            if (tags == null || tags.Count == 0)
            {
                return null;
            }

            var items = new List<TagItem>();
            foreach (var item in tags)
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }
                items.Add(new TagItem { TagName = item });
            }
            return items;
        }

        static string[] GetComments(Posts post)
        {
            if (post.Comments == null || post.Comments.Count == 0)
            {
                return null;
            }

            string[] comments = new string[3];
            comments[0] = "0";
            comments[1] = post.Comments.Count.ToString();
            comments[2] = "0";
            //comments[0] = post.NotApprovedComments.Count.ToString();
            //comments[1] = post.ApprovedComments.Count.ToString();
            //comments[2] = post.SpamComments.Count.ToString();
            return comments;
        }


        //static string Gravatar(Comment comment)
        //{
        //    var website = comment.Website == null ? "" : comment.Website.ToString();
        //    return AvatarService.GetSrc(comment.Email, website);
        //}

        //#endregion

    }
}
