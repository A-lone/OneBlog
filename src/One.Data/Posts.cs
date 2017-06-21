using System;
using System.Collections.Generic;
using One.Helpers;

namespace One.Data
{
    /// <summary>
    /// 文章列表
    /// </summary>
    public class Posts
    {
        public Posts()
        {
            Id = GuidComb.GenerateComb();
        }

        public Guid Id { get; set; }
        /// <summary>
        /// 封面图片
        /// </summary>
        public string CoverImage { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string Description { get; set; }
        public DateTime DatePublished { get; set; }
        public bool IsPublished { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }

        public bool HasCommentsEnabled { get; set; }

        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool HasRecommendEnabled { get; set; }

        public long Count { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public virtual ApplicationUser Author { get; set; }

        public virtual IList<PostsInCategories> PostsInCategories { get; set; }

        public virtual IList<Comments> Comments { get; set; }

        public virtual IList<TagsInPosts> TagsInPosts { get; set; }

    }
}