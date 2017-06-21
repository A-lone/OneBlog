using Microsoft.AspNetCore.Mvc;
using One.Mock.Data;
using System;
using System.Collections.Generic;

namespace One.Mock.Repositories
{
    public interface ISitePathRepository
    {
        List<SitePath> GetAll();

        List<SitePath> Get(Guid id);

        void Post(SitePath sitePath);

        void Put(Guid id, [FromBody]SitePath sitePath);

        void Delete(Guid id);
    }
}