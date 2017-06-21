using One.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace One.Data.Models
{
    /// <summary>
    /// Comments view model
    /// </summary>
    public class CommentsResult
    {
        public List<CommentItem> Items { get; set; }
        public CommentItem SelectedItem { get; set; }
        public CommentDetail Detail { get; set; }
    }
}
