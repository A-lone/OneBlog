using One.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace One.Data
{

    /// <summary>
    /// 应用商店分类
    /// </summary>
    [DataContract]
    public class StoreCategories
    {
        public StoreCategories()
        {
            Id = GuidComb.GenerateComb();
        }
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Description { get; set; }

        [IgnoreDataMember]
        public virtual IList<StoreApp> StoreApp { get; set; }

    }
}
