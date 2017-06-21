using System.ComponentModel.DataAnnotations.Schema;

namespace One.AspNetCore.TimedJob.EntityFramework
{
    [Table("AspNetTimedJobs")]
    public class TimedJob : DynamicTimedJob
    {
    }
}
