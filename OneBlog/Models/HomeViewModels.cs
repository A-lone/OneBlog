using OneBlog.Core;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace OneBlog.Models
{
    public class HomeViewModels
    {
        public List<IPublishable> Posts { get; set; }

        public IPublishable CoverPost { get; set; }

    }
    public class CommentViewModels
    {

        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Captcha { get; set; }

        public Guid PostId { get; set; }

        public List<Comment> Comments
        {
            get; set;
        }

    }


    public class MenuViewModels
    {
        public int? PageIndex { get; set; }
        public int? TotalCount { get; set; }
        public int? TotalPages { get; set; }

        public List<IPublishable> Posts { get; set; }

    }




    public class PostViewModels
    {
        public Post Post { get; set; }
    }


    public class ContactViewModels
    {
        [Required]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "主题")]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "消息")]
        public string Message { get; set; }

        [Display(Name = "附件")]
        public HttpPostedFile Attachment { get; set; }
    }


    public class ArchiveViewModels
    {

        /// <summary>
        /// 评论
        /// </summary>
        public int CommentsCount { get; set; }

        /// <summary>
        /// 评分者
        /// </summary>
        public int RatersCount { get; set; }

        /// <summary>
        /// 文章
        /// </summary>
        public int PostsCount { get; set; }


        public List<Category> Category { get; set; }


    }

}