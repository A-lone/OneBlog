using OneBlog.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OneBlog.Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace OneBlog.Data.Repository
{
    public class TagsRepository : BaseRepository, ITagsRepository
    {
        private readonly IDbContextFactory _contextFactory;

        public TagsRepository(IDbContextFactory contextFactory, IConfigurationRoot config)
        {
            _contextFactory = contextFactory;
        }

        public TagItem Add(TagItem item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TagItem> Find(int take = 10, int skip = 0, string filter = "", string order = "")
        {
            // get post categories with counts
            var items = new List<TagItem>();
            using (var ctx = _contextFactory.Create())
            {
                var tags = ctx.Tags.Include(m => m.TagsInPosts).Where(m => m.TagsInPosts.Count > 0)
                .OrderByDescending(m => m.TagsInPosts.Count).ToList();
                // add categories without posts
                foreach (var c in tags)
                {
                    items.Add(new TagItem { TagName = c.TagName, TagCount = c.TagsInPosts.Count });
                }

                if (take == 0)
                {
                    take = items.Count;
                }
            }

            return items.Skip(skip).Take(take);
        }

        public TagItem FindById(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<TagItem> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool Update(TagItem item)
        {
            throw new NotImplementedException();
        }
    }
}
