using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsEngine2._0.Models
{
    public class News
    {
        [Key]
        public int NewsId { get; set; }
        [Required(ErrorMessage = "Titlul este obligatoriu")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Continutul este obligatoriu")]
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required(ErrorMessage ="Categoria este obligatorie")]
        public int CategoryId { get; set; } 
        public bool IsActive { get; set; }
        public ApplicationUser User { get; set; }
        public Category Category { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
    }

}