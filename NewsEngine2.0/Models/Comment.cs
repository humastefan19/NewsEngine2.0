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
        [Required]
        public int UserId{ get; set; }
        [Required (ErrorMessage ="Nu ati introdus comentariu")]
        public string Content { get; set; }

        public ApplicationUser User{ get; set; }
    }
}