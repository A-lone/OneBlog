using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace One.Data
{
    public class PostsInCategories
    {
        public PostsInCategories()
        {
        }

        public Guid PostsId { get; set; }

        public Guid CategoriesId { get; set; }

        public virtual Posts Posts { get; set; }

        public virtual Categories Categories { get; set; }
    }
}
