using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsEngine2._0.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        
        public string UserId { get; set; }
        public int NewsId { get; set; }
        [Required (ErrorMessage ="Continutul comentariului este obligatoriu")]
        public string Content { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual News News { get; set; }
    }
}