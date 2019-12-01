using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsEngine2._0.Models
{
    public class ProposedNews
    {
        [Key]
        public int ProposedNewsId { get; set; }
        [Required]
        public string UserId {get; set; }
        [Required(ErrorMessage ="Nu ati introdus titlul")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Nu ati introdus continutul")]
        public string Content { get; set; }
        [Required]
        public int CategoryId { get; set; }

        public ApplicationUser User { get; set; }
        public Category Category { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}