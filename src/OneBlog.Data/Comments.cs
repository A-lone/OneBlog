using One.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace One.Data
{
    /// <summary>
    /// 评论
    /// </summary>
    public class Comments
    {
        public Comments()
        {
            Id = GuidComb.GenerateComb();
        }

        public Guid Id { get; set; }

        public string Content { get; set; }

        public DateTime CommentDate { get; set; }

        public virtual ApplicationUser Author { get; set; }

        /// <summary>
        /// Ip地址
        /// </summary>
        public string Ip { get; set; }

        public Guid ParentId { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the Comment is approved.
        /// </summary>
        public bool IsApproved { get; set; }

        public virtual Posts Posts { get; set; }

        /// <summary>
        ///     Indicate if comment is spam
        /// </summary>
        public bool IsSpam { get; set; }


    }
}
