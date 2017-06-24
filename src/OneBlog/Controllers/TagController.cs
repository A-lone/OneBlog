using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OneBlog.Data;
using OneBlog.Data.Contracts;
using OneBlog.Settings;

namespace OneBlog.Controllers
{
    [Route("[controller]")]
    public class TagController : Controller
    {
        private readonly IPostsRepository _repo;
        private readonly IOptions<AppSettings> _appsettings;

        public TagController(IOptions<AppSettings> appsettings, IPostsRepository repo)
        {
            _appsettings = appsettings;
            _repo = repo;
        }

        [HttpGet("{tag}")]
        public IActionResult Index(string tag)
        {
            return Pager(tag, 1);
        }

        [HttpGet("{tag}/{page}")]
        public IActionResult Pager(string tag, int page)
        {
            return View("Index", _repo.GetPostsByTag(tag, _appsettings.Value.PostPerPage, page));
        }
    }
}