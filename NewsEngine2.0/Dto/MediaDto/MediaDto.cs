using NewsEngine2._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsEngine2._0.Dto.MediaDto
{
    public class MediaDto
    {
          public virtual News News { get; set; }
          public List<Media> Medias { get; set; }
    }
}