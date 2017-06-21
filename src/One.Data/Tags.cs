using System;
using One.Helpers;
using System.Collections.Generic;

namespace One.Data
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
