using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OneBlog.Data
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
