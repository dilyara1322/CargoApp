using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CargoApp.Models
{
    //[Owned]
    //abstract
    public class UserRegData
    {
        
        [Required]
        [Key]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Salt { get; set; }
        public string Name { get; set; }
    }
}
