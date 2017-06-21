using OneBlog.Data.Models;
using System;
using System.Collections.Generic;

namespace OneBlog.Data.Contracts
{
    public interface IStoreRepository
    {
        IList<StoreApp> GetApps(Guid? categoryId = default(Guid?));
        IList<StoreCategories> GetCategories();

        StoreAppResult GetApps(Guid? categoryId = null, int pageSize = 16, int page = 1);
    }
}