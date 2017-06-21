using System;
using System.ComponentModel.DataAnnotations;

namespace OneBlog.AspNetCore.TimedJob
{
    public class DynamicTimedJob
    {
        [MaxLength(512)]
        public string Id { get; set; }

        public DateTime Begin { get; set; }

        public int Interval { get; set; }

        public bool IsEnabled { get; set; }
    }
}
