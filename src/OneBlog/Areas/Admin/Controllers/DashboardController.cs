using Microsoft.AspNetCore.Mvc;
using OneBlog.Data.Contracts;
using OneBlog.Data.Models.ManageViewModels;

namespace OneBlog.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    public class DashboardController : Controller
    {
        readonly IDashboardRepository repository;

        public DashboardController(IDashboardRepository repository)
        {
            this.repository = repository;
        }

        public DashboardViewModel Get()
        {
            return repository.Get();
        }
    }
}
