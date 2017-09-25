using System;
using System.Collections.Generic;
using System.Text;

namespace OneBlog.Configuration
{
    public class DataSettings
    {
        public string ConnectionString { get; set; }

        public DataProvider Provider { get; set; }
    }
}
