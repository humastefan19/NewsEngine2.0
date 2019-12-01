using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsEngine2._0.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set;}
        [Required(ErrorMessage ="Numele categoriei este obligatoriu")]
        public string Name { get; set; }

        public ICollection<News> News { get; set; }
    }
}