using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using One.Mock.Data;
using One.Mock.ViewModels;

namespace One.Mock.Repositories
{
    public interface ISiteRepository
    {
        void Delete(Guid id);
        Site Get(Guid id);
        List<Site> GetAll();
        Site GetDefault();
        void Post(SiteVM site);
        void Put(Guid id, [FromBody] SiteVM site);
    }
}