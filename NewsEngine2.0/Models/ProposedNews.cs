﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsEngine2._0.Models
{
    public class ProposedNews
    {
        public int ProposedNewsId { get; set; }
        public int UserId{ get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int CategoryId { get; set; }
    }
}