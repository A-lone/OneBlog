using System;
using System.Collections.Generic;
using System.Text;

namespace OneBlog.Configuration
{
    public class QiniuSettings
    {
        public string AccessKey { get; set; }

        public string SecretKey { get; set; }

        public string Bucket { get; set; }

        public string UPHost { get; set; }

        public string Domain { get; set; }
    }
}
