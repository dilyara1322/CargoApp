using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CargoApp.Models
{
    [Owned]
    public class Address
    {
        // [Required]
        // public int Id { get; set; }
        /*
         [Required]
         public int Latitude { get; set; } //умноженное на 1 000 000
         [Required]
         public int Longitude { get; set; } //умноженное на 1 000 000
         
        [NotMapped]
        public float RealLatitude { get; set; } //реальное значение, не хранится в базе
        [NotMapped]
        public float RealLongitude { get; set; } //реальное значение, не хранится в базе
*/

        [Column(TypeName = "char(6)")]
        public string Index { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
       // public string Part { get; set; }
        public string Flat { get; set; }

        public string Addition { get; set; }
    }
}
