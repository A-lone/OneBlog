using One.Mock.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace One.Mock.ViewModels
{
    [DataContract]
    public class SiteVM
    {
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
    }
}
