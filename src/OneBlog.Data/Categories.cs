using System;
using System.Collections.Generic;
using One.Helpers;

namespace One.Data
{
    public class Categories
    {

        public Categories()
        {
            Id = GuidComb.GenerateComb();
        }

        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Guid ParentId { get; set; }

        public virtual IList<PostsInCategories> PostsInCategories { get; set; }
    }
}
