using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsEngine2._0.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int UserId{ get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}