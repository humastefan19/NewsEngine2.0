using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsEngine2._0.Models
{
    public class Media
    {
        public int MediaId { get; set; }
        public int NewsId { get; set; }
        public int MediaTypeId { get; set; }
        public string Path { get; set; }

        public virtual MediaType MediaType { get; set; }
        public virtual News News { get; set; }
    }
}