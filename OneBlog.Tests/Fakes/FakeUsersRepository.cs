﻿using OneBlog.Core.Data.Contracts;
using OneBlog.Core.Data.Models;
using System.Collections.Generic;

namespace OneBlog.Tests.Fakes
{
    class FakeUsersRepository : IUsersRepository
    {
        public IEnumerable<BlogUser> Find(int take = 10, int skip = 0, string filter = "", string order = "")
        {
            var items = new List<BlogUser>();
            items.Add(new BlogUser());
            return items;
        }

        public BlogUser FindById(string id)
        {
            return new BlogUser();
        }

        public BlogUser Add(BlogUser user)
        {
            return new BlogUser();
        }

        public bool Update(BlogUser user)
        {
            return true;
        }

        public bool SaveProfile(BlogUser user)
        {
            return true;
        }

        public bool Remove(string id)
        {
            return true;
        }
    }
}
