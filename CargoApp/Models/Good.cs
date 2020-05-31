using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CargoApp.Models
{
    public class Good
    {
        [Required]
        public int Id { get; set; }
        public enum GoodType { Usual, Fragile, Perishable, Alive, Oversized, Dangerous, Other }
        public GoodType? Type { get; set; }
        [Required]
        public string Name { get; set; }
        public float? Weight { get; set; }
        public float? Length { get; set; }
        public float? Height { get; set; }
        public float? Width { get; set; }
        //public bool? IsFragile { get; set; }

        [Required]
        public int RequestId { get; set; }
        public Request Request { get; set; }

    }
}
