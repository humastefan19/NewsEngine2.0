using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsEngine2._0.Models
{
    public class MediaType
    {
        [Key]
        public int MediaTypeId { get; set; }
        [Required(ErrorMessage ="Numele tipului de fisier trebuie specificat")]
        public string Name { get; set; }

        public ICollection<Media> Media { get; set; }
    }
}