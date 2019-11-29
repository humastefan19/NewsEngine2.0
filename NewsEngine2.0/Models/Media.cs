using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsEngine2._0.Models
{
    public class Media
    {
        [Key]
        public int MediaId { get; set; }
        [Required]
        public string FilePath { get; set; }
        [Required]
        public int NewsId { get; set; }
        [Required]
        public int MediaTypeId { get; set; }

        public News News { get; set; }
        public MediaType MediaType { get; set; }
    }
}