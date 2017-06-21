using System;
using OneBlog.Helpers;
using System.Collections.Generic;

namespace OneBlog.Data
{
    public class Tags
    {
        public Tags()
        {
            Id = GuidComb.GenerateComb();
        }

        public Guid Id { get; set; }

        public string TagName { get; set; }

        public virtual IList<TagsInPosts> TagsInPosts { get; set; }
    }
}
