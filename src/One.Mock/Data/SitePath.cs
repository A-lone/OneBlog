using One.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace One.Mock.Data
{
    public class SitePath
    {
        public SitePath()
        {
            Id = GuidComb.GenerateComb();
        }
        public Guid Id { get; set; }
        public string Path { get; set; }
        public string Method { get; set; }

        public string Query { get; set; }

        public string Cookie { get; set; }

        public string Json { get; set; }

        public string Expression { get; set; }

        public string DLL { get; set; }

        public bool RequestEnabled { get; set; }

        public virtual Site Sites { get; set; }
    }
}
