using System;
using System.Collections.Generic;
using System.Text;

namespace OneBlog.Configuration
{
    public class DataSettings
    {
        public string ConnectionString { get; set; }

        public string ConnectionString_Debug { get; set; }

        public DataProvider Provider { get; set; }
    }
}
