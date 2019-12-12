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
<<<<<<< HEAD
        [Required]
        public int UserId{ get; set; }
        [Required (ErrorMessage ="Nu ati introdus comentariu")]
        public string Content { get; set; }

        public ApplicationUser User{ get; set; }
=======
        public string UserId { get; set; }
        public int NewsId { get; set; }
        [Required (ErrorMessage ="Continutul comentariului este obligatoriu")]
        public string Content { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual News News { get; set; }
>>>>>>> 6bef09f5513871781213befc4129b952754dfedb
    }
}