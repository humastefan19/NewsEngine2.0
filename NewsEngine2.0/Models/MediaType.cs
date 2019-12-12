using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsEngine2._0.Models
{
    public class MediaType
    {
        public int MediaTypeId { get; set; }
<<<<<<< HEAD
        [Required(ErrorMessage ="Numele tipului de fisier trebuie specificat")]
=======
        [Required]
>>>>>>> 6bef09f5513871781213befc4129b952754dfedb
        public string Name { get; set; }
    }
}