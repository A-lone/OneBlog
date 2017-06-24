using System;
using OneBlog.Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using OneBlog.Data.Common;
using System.Collections.Generic;
using OneBlog.Helpers;
using Microsoft.AspNetCore.Identity;

namespace OneBlog.Data.Contracts
{
    /// <summary>
    /// Comments repository
    /// </summary>
    public class CommentsRepository : ICommentsRepository
    {

        private readonly IDbContextFactory _contextFactory;
        private JsonService _jsonService;
        private readonly UserManager<ApplicationUser> _userManager;
        public CommentsRepository(IDbContextFactory contextFactory, IConfigurationRoot config, JsonService jsonService, UserManager<ApplicationUser> userManager)
        {
            _contextFactory = contextFactory;
            _jsonService = jsonService;
            _userManager = userManager;
        }

        public CommentItem Add(CommentDetail item)
        {
            var c = new Comments();
            try
            {
                using (var ctx = _contextFactory.Create())
                {
                    var post = ctx.Posts.Where(p => p.Id == item.PostId).FirstOrDefault();
                    c.CommentDate = DateTime.Now;
                    c.ParentId = item.ParentId;
                    c.IsApproved = true;
                    c.Content = item.Content;

                    if (string.IsNullOrEmpty(item.Author.Id))
                    {
                        var guid = Guid.NewGuid().ToString().Replace("-", "");
                        var user = new ApplicationUser { UserName = "anonymous_" + guid, Email = item.Author.Email, DisplayName = "匿名_" + item.Author.DisplayName };
                        user.Avatar = AvatarHelper.GetRandomAvatar();
                        var result = _userManager.CreateAsync(user).Result;
                        if (!result.Succeeded)
                        {
                            return null;
                        }
                        item.Author = user;
                    }

                    c.Author = item.Author;
                    c.Ip = AspNetCoreHelper.GetRequestIP();
                    c.Posts = post;
                    ctx.Comments.Add(c);
                    ctx.SaveChanges();
                    //var profile = AuthorProfile.GetProfile(c.Author);
                    //if (profile != null && !string.IsNullOrEmpty(profile.DisplayName))
                    //{
                    //    c.Author = profile.DisplayName;
                    //}
                    //c.Email = Membership.Provider.GetUser(Security.CurrentUser.Identity.Name, true).Email;
                    //c.IP = WebUtils.GetClientIP();
                    //c.DateCreated = DateTime.Now;
                    //c.Parent = post;
                    var newComm = post.Comments.Where(cm => cm.Content == c.Content).FirstOrDefault();
                    return _jsonService.GetComment(newComm, post.Comments.ToList());
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool DeleteAll(string commentType)
        {
            throw new NotImplementedException();
        }

        public CommentDetail FindById(Guid id)
        {
            using (var ctx = _contextFactory.Create())
            {
                return (from p in ctx.Posts
                        from c in p.Comments
                        where c.Id == id
                        select _jsonService.GetCommentDetail(c)).FirstOrDefault();
            }
        }

        public List<CommentItem> FindByPostId(Guid postId)
        {
            using (var ctx = _contextFactory.Create())
            {
                var comments = ctx.Comments.Include(m => m.Author).Include(m => m.Posts).Where(m => m.Posts.Id == postId).ToList();
                // instantiate object
                var nestedComments = new List<CommentItem>();

                // temporary ID/Comment table
                var commentTable = new Dictionary<Guid, CommentItem>();

                foreach (var comment in comments)
                {
                    var commentIten = _jsonService.GetComment(comment, comments);
                    // add to hashtable for lookup
                    commentTable.Add(comment.Id, commentIten);

                    // check if this is a child comment
                    if (comment.ParentId == Guid.Empty)
                    {
                        // root comment, so add it to the list
                        nestedComments.Add(commentIten);
                    }
                    else
                    {
                        // child comment, so find parent
                        var parentComment = commentTable[comment.ParentId] as CommentItem;
                        if (parentComment != null)
                        {
                            // double check that this sub comment has not already been added
                            if (parentComment.Comments.IndexOf(commentIten) == -1)
                            {
                                parentComment.Comments.Add(commentIten);
                            }
                            //parentComment.Comments = parentComment.Comments.OrderByDescending(m => m.DateCreated).ToList();
                        }
                        else
                        {
                            // just add to the base to prevent an error
                            nestedComments.Add(commentIten);
                        }
                    }
                }
                //降序排序
                return nestedComments.OrderByDescending(m => m.DateCreated).ToList();
            }
        }

        public CommentsResult Get()
        {
            var vm = new CommentsResult();
            using (var ctx = _contextFactory.Create())
            {
                var comments = ctx.Comments.Include(m => m.Author).ToList();
                var items = new List<CommentItem>();
                foreach (var c in comments)
                {
                    items.Add(_jsonService.GetComment(c, comments));
                }
                vm.Items = items;
                vm.Detail = new CommentDetail();
                vm.SelectedItem = new CommentItem();
            }
            return vm;
        }

        public List<Comments> GetChildren(List<Comments> list)
        {
            List<Comments> temp = new List<Comments>();
            using (var ctx = _contextFactory.Create())
            {
                foreach (var item in list)
                {
                    var newList = ctx.Comments.Where(m => m.ParentId == item.Id).ToList();
                    newList = GetChildren(newList);
                    temp.AddRange(newList);
                }
            }
            list.AddRange(temp);
            return list;
        }

        public bool Remove(Guid id)
        {
            using (var ctx = _contextFactory.Create())
            {
                var item = (from cmn in ctx.Comments
                            where cmn.Id == id
                            select cmn).ToList();
                if (item != null && item.Count > 0)
                {
                    var list = GetChildren(item);
                    ctx.Comments.RemoveRange(list);
                    ctx.SaveChanges();
                    return true;
                }
                return false;
            }
        }


        public bool Update(CommentItem item, string action)
        {
            throw new NotImplementedException();
        }
    }
}