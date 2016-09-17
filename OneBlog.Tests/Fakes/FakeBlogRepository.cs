using OneBlog.Core.Data.Contracts;
using OneBlog.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBlog.Tests.Fakes
{
    class FakeBlogRepository : IBlogRepository
    {
        public IEnumerable<Core.Data.Models.Blog> Find(int take = 10, int skip = 0, string filter = "", string order = "")
        {
            var items = new List<OneBlog.Core.Data.Models.Blog>();

            items.Add(new OneBlog.Core.Data.Models.Blog()
            {
	            Id = Guid.NewGuid()
            });

            return items;
        }

        public Core.Data.Models.Blog FindById(Guid id)
        {
            var blog = new OneBlog.Core.Data.Models.Blog()
            {
                Id = Guid.NewGuid()
            };
            return blog;
        }

        public Core.Data.Models.Blog Add(Core.Data.Models.BlogItem item)
        {
            var blog = new OneBlog.Core.Data.Models.Blog()
            {
                Id = Guid.NewGuid()
            };
            return blog;
        }

        public bool Update(Core.Data.Models.Blog blog)
        {
            return true;
        }

        public bool Remove(Guid id)
        {
            return true;
        }
    }
}
