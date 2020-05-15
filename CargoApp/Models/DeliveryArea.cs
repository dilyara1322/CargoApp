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
        [NotMapped]
        private int? latitude;
        [Required]
        public int? Latitude { get { return latitude; }
            set { latitude = value; realLatitude = value / 1000000; } } //умноженное на 1 000 000
        [NotMapped]
        private int? longitude;
        [Required]
        public int? Longitude { get { return longitude; }
            set { longitude = value; realLongitude = value / 1000000; } } //умноженное на 1 000 000

        [NotMapped]
        private float? realLatitude;
        [NotMapped]
        public float? RealLatitude { get { return realLatitude; } 
            set { realLatitude = value; latitude = (int)(value * 1000000); } } //реальное значение, не хранится в базе
        [NotMapped]
        private float? realLongitude;

        [NotMapped]
        public float? RealLongitude { get { return realLongitude; }
            set { realLongitude = value; longitude = (int)(value * 1000000); } } //реальное значение, не хранится в базе

        [Required]
        public int? Radius { get; set; } //meters
    }
}
