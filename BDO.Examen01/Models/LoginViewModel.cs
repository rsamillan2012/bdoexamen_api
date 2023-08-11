using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BDO.Examen01.Models
{
    public class LoginViewModel
    {
        public int id { get; set; }
        //[Required]
        //[EmailAddress]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        public string nuevopassword { get; set; }

        public string keymaster { get; set; }
    }
}
