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
        public string UserId {get; set; }
        [Required(ErrorMessage ="Nu ati introdus titlul")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Nu ati introdus continutul")]
        public string Content { get; set; }
        [Required(ErrorMessage ="Categoria este obligatorie")]
        public int CategoryId { get; set; }
        [DataType(DataType.DateTime, ErrorMessage = "Campul trebuie sa contina data si ora")]
        public DateTime CreatedDate { get; set; }

        public virtual ApplicationUser User { get; set; }
<<<<<<< HEAD
        public Category Category { get; set; }
=======
        public virtual Category Category { get; set; }
>>>>>>> e73f4497f08ca1144bb4c999469ce9356fed7972
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}