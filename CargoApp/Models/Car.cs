using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CargoApp.Models
{
   // [Table("Cars")]
    public class Car
    {
        [Key]
        public int Id { get; set; }
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [MaxLength(12)]
        public string Number { get; set; }
        public string Model { get; set; }
        [Required]
        public float? Volume { get; set; }
        [Required]
        public float? Carrying { get; set; }
        [Required]
        public int DriverId { get; set; }
        public Driver Driver { get; set; }
    }
}
