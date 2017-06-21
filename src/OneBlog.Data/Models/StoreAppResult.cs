using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace OneBlog.Data.Models
{
    [DataContract]
    public class StoreAppResult : BasePager
    {
        [DataMember]
        public IList<StoreApp> Apps { get; set; }

        [DataMember]
        public string Category { get; set; }
    }
}
