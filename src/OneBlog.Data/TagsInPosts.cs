using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneBlog.Data
{
    public class TagsInPosts
    {
        public TagsInPosts()
        {
        }

        public Guid TagId { get; set; }

        public Guid PostId { get; set; }

        public virtual Posts Posts { get; set; }

        public virtual Tags Tags { get; set; }
    }
}
