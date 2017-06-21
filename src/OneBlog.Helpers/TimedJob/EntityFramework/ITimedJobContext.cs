using Microsoft.EntityFrameworkCore;

namespace OneBlog.AspNetCore.TimedJob.EntityFramework
{
    public interface ITimedJobContext
    {
        DbSet<TimedJob> TimedJobs { get; set; }

        int SaveChanges();
    }
}
