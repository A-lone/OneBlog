using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace One.Mock.Model
{
    [DataContract]
    public class Expression
    {
        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public string IFKey { get; set; }

        [DataMember]
        public string IFValue { get; set; }
    }
}
