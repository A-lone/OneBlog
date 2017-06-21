using Microsoft.AspNetCore.Mvc;
using One.Mock.Data;
using One.Mock.Repositories;
using System;

namespace One.Mock.Controllers
{
    [Route("mock/admin/[controller]")]
    public class SitePathController : Controller
    {
        private readonly ISitePathRepository _sitePathRepository;

        public SitePathController(ISitePathRepository siteRepository)
        {
            _sitePathRepository = siteRepository;
        }


        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var data = _sitePathRepository.Get(id);
            return Json(new { ErrNo = 0, ErrMsg = "", Data = data });
        }

        [HttpPost]
        public void Post([FromBody]SitePath value)
        {
            _sitePathRepository.Post(value);
        }

        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody]SitePath value)
        {
            _sitePathRepository.Put(id, value);
        }

        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            _sitePathRepository.Delete(id);
        }
    }
}
