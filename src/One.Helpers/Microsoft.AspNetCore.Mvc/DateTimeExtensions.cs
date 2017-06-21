using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Mvc
{
    public static class DateTimeExtensions
    {
        public static DateTime ToDateTime(this long t)
        {
            DateTime time = new DateTime(0x7b2, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return time.AddSeconds((double)t).ToLocalTime();
        }
    }
}
