using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CargoApp.Models
{
    public class Company
    {
        [Required]
        public int Id { get; set; }
        //[Required]
        public string Name { get; set; }
        public float? Rating { get; set; }

        public float? MaxCarrying { get; set; }
        //public float? MaxVolume { get; set; }
        public DeliveryArea Area { get; set; }

        public List<Logistician> Logisticians { get; set; }
        public List<Driver> Drivers { get; set; }

        public Address Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime? RegistrationDate { get; set; }

        [MinLength(10)]
        [MaxLength(12)]
        public string Inn { get; set; }
        //[MinLength(9)]
        //[MaxLength(9)]
        [Column(TypeName = "char(9)")]
        public string Kpp { get; set; }
        //[MinLength(13)]
        //[MaxLength(13)]
        [Column(TypeName = "char(13)")]
        public string Ogrn { get; set; }
      //  public DateTime? OgrnDate { get; set; }

        public List<Rating> ClientsMarks { get; set; }
        public List<Request> Requests { get; set; }
    }
}
