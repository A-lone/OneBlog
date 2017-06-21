using One.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace One.Mock.Data
{

    [DataContract]
    public class Site
    {
        public Site()
        {
            Id = GuidComb.GenerateComb();
        }
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Url { get; set; }
        [DataMember]
        public string Cookie { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public bool IsDefault { get; set; }

        //public string UserAgent { get; set; }
        [IgnoreDataMember]
        public virtual IList<SitePath> SitePaths { get; set; }
    }
}
