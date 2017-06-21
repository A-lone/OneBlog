using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace One.Data.Models
{
    /// <summary>
    /// Used for dropdowns
    /// </summary>
    [DataContract]
    public class SelectOption
    {
        /// <summary>
        /// Option name
        /// </summary>
        /// 
        [DataMember]
        public string OptionName { get; set; }
        /// <summary>
        /// Option value
        /// </summary>
        [DataMember]

        public string OptionValue { get; set; }
        /// <summary>
        /// Is option selected
        /// </summary>
        [DataMember]
        public bool IsSelected { get; set; }
    }
}
