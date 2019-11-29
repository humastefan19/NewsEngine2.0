using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsEngine2._0.Models
{
    public class User
    {
        public int Id { get; set; }
        public int IdTip { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public DateTime DataNastere { get; set; }
        public string Email { get; set; }
        public string NumarTelefon { get; set; }
        public string Username { get; set; }
        public short Activ { get; set; }
   
    }
}
