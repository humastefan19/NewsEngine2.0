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
        [Required(ErrorMessage ="Calea fisierului media este obligatorie")]
        public string FilePath { get; set; }
        [Required(ErrorMessage ="Strirea asociata fisierului media este obligatoriu")]
        public int NewsId { get; set; }
        [Required(ErrorMessage ="Fisierul Media trebuie sa aiba un tip")]
        public int MediaTypeId { get; set; }

        public News News { get; set; }
        public MediaType MediaType { get; set; }
    }
}