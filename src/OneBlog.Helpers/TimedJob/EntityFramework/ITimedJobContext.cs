using Microsoft.EntityFrameworkCore;

namespace One.AspNetCore.TimedJob.EntityFramework
{
    public interface ITimedJobContext
    {
        DbSet<TimedJob> TimedJobs { get; set; }

        int SaveChanges();
    }
}
