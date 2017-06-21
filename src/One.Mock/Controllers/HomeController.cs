using Microsoft.AspNetCore.Mvc;
using One.Mock.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace One.Mock.Controllers
{
    [Route("mock/admin")]
    public class HomeController : Controller
    {
        private readonly ISiteRepository _siteRepository;
        private readonly ISitePathRepository _sitePathRepository;

        public HomeController(ISiteRepository siteRepository, ISitePathRepository sitePathRepository)
        {
            _siteRepository = siteRepository;
            _sitePathRepository = sitePathRepository;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var data = _siteRepository.GetAll();
            return View(data);
        }


        [HttpGet("rule/{id?}")]
        public IActionResult Rule(Guid id)
        {
            var data = _sitePathRepository.Get(id);
            return View(data);
        }
    }
}
