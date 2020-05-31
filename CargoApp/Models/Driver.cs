using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CargoApp.Models
{
    public class Driver //: User
    {
        [Required]
        public int Id { get; set; }
       // [MaxLength(30)]
        public string Login { get; set; }
        [ForeignKey("Login")]
        public UserRegData RegData { get; set; }
        public string PhoneNumber { get; set; }
      //  [NotMapped]
        //public bool IsFree { get; set; }
        [Required]
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        //public Passport Passport { get; set; }

        /*
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Salt { get; set; }

        public string Name { get; set; }
        */

        public List<Car> Cars { get; set; }
        public DeliveryArea DeliveryArea { get; set; }
        //public ГрафикРаботы 

        public List<Request> Requests { get; set; }
    }
}
