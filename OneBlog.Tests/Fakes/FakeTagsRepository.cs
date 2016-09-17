using OneBlog.Core.Data.Contracts;
using OneBlog.Core.Data.Models;
using System.Collections.Generic;

namespace OneBlog.Tests.Fakes
{
    class FakeTagsRepository : ITagRepository
    {
        public IEnumerable<TagItem> Find(int take = 10, int skip = 0, string postId = "", string order = "")
        {
            var items = new List<TagItem>();
            items.Add(new TagItem());
            return items;
        }

        public bool Save(string updateFrom, string updateTo)
        {
            return true;
        }

        public bool Delete(string id)
        {
            return true;
        }
    }
}
