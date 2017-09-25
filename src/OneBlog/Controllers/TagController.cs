using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OneBlog.Configuration;
using OneBlog.Data.Contracts;

namespace OneBlog.Controllers
{
    [Route("[controller]")]
    public class TagController : Controller
    {
        private IPostsRepository _repo;
        private IOptions<AppSettings> _appSettings;

        public TagController(IPostsRepository repo, IOptions<AppSettings> appSettings)
        {
            _repo = repo;
            _appSettings = appSettings;
        }

        [HttpGet("{tag}")]
        public IActionResult Index(string tag)
        {
            return Pager(tag, 1);
        }

        [HttpGet("{tag}/{page}")]
        public IActionResult Pager(string tag, int page)
        {
            return View("Index", _repo.GetPostsByTag(tag, _appSettings.Value.PostPerPage, page));
        }
    }
}