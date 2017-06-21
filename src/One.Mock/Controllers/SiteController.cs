using Microsoft.AspNetCore.Mvc;
using One.Mock.Data;
using One.Mock.Repositories;
using One.Mock.ViewModels;
using System;

namespace One.Mock.Controllers
{
    [Route("mock/admin/[controller]")]
    public class SiteController : Controller
    {
        private readonly ISiteRepository _siteRepository;

        public SiteController(ISiteRepository siteRepository)
        {
            _siteRepository = siteRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var data = _siteRepository.GetAll();
            return Json(new { ErrNo = 0, ErrMsg = "", Data = data });
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var data = _siteRepository.Get(id);
            return Json(new { ErrNo = 0, ErrMsg = "", Data = data });
        }

        [HttpPost]
        public IActionResult Post([FromBody]SiteVM item)
        {
            _siteRepository.Post(item);
            return Get();
        }

        [HttpPost("update")]
        public IActionResult Update([FromBody]SiteVM item)
        {
            _siteRepository.Put(item.Id, item);
            return Get();
        }

        [HttpPost("delete/{id?}")]
        public void Delete(Guid id)
        {
            _siteRepository.Delete(id);
        }
    }
}
