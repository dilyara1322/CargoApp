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
    public class DeliveryArea
    {
        [Required]
        public int Latitude { get; set; } //умноженное на 1 000 000
        [Required]
        public int Longitude { get; set; } //умноженное на 1 000 000

        [NotMapped]
        public float RealLatitude { get; set; } //реальное значение, не хранится в базе
        [NotMapped]
        public float RealLongitude { get; set; } //реальное значение, не хранится в базе

        [Required]
        public int Radius { get; set; } //meters
    }
}
