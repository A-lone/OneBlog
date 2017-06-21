using System.ComponentModel.DataAnnotations.Schema;

namespace OneBlog.AspNetCore.TimedJob.EntityFramework
{
    [Table("AspNetTimedJobs")]
    public class TimedJob : DynamicTimedJob
    {
    }
}
