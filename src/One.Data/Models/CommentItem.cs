using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace One.Data.Models
{
    public class CommentItem
    {
        /// <summary>
        /// If checked in the UI
        /// </summary>
        public bool IsChecked { get; set; }
        /// <summary>
        ///     Gets or sets the Comment Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        ///     If comment is pending
        /// </summary>
        public bool IsPending { get; set; }
        /// <summary>
        ///     If comment is approved
        /// </summary>
        public bool IsApproved { get; set; }
        /// <summary>
        ///     Whether comment is spam
        /// </summary>
        public bool IsSpam { get; set; }
        /// <summary>
        ///     Gets or sets the Author
        /// </summary>
        public Author Author { get; set; }
        /// <summary>
        ///     Whether this comment has nested comments
        /// </summary>
        public bool HasChildren { get; set; }
        /// <summary>
        ///     Gets or sets the date published
        /// </summary>
        public string DateCreated { get; set; }

        public string Content { get; set; }

        /// <summary>
        /// Content的缩略
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The comments.
        /// </summary>
        private List<CommentItem> comments;

        public List<CommentItem> Comments
        {
            get
            {
                return comments ?? (comments = new List<CommentItem>());
            }
            set
            {
                comments = value;
            }
        }
    }
}
