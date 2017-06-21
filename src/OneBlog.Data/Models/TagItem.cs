using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace OneBlog.Data.Models
{
    /// <summary>
    /// The tag item
    /// </summary>
    [DataContract]
    public class TagItem
    {
        /// <summary>
        /// If checked in the UI
        /// </summary>
        [DataMember]
        public bool IsChecked { get; set; }
        /// <summary>
        /// Tag Name
        /// </summary>
        [DataMember]
        public string TagName { get; set; }
        /// <summary>
        /// Tag Count
        /// </summary>
        [DataMember]
        public int TagCount { get; set; }
    }
}
