using OneBlog.Core.Data.Contracts;
using OneBlog.Core.Data.Models;
using System.Web.Http;

public class StatsController : ApiController
{
    readonly IStatsRepository repository;

    public StatsController(IStatsRepository repository)
    {
        this.repository = repository;
    }

    public Stats Get()
    {
        return repository.Get();
    }
}
