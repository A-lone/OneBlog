using OneBlog.Core.Data.Contracts;
using OneBlog.Core.Data.Models;

namespace OneBlog.Tests.Fakes
{
    class FakeStatsRepository : IStatsRepository
    {
        public Stats Get()
        {
            return new Stats();
        }
    }
}
