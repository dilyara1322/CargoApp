using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CargoApp.Models
{
    public class Client //: UserRegData
    {
        /*
        [Required]
        public int Id { get; set; }

        public string Name { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Salt { get; set; }
        */

        [Required]
        public int Id { get; set; }
        public float Rating { get; set; }
        public string Login { get; set; }
        [ForeignKey("Login")]
        public UserRegData RegData { get; set; }
        public Passport Passport { get; set; }
      //  public Address Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        //public string AccountNumber { get; set; }
       // public PayData PayData { get; set; }

        public List<Request> Requests { get; set; }
        public List<Rating> CompaniesMarks { get; set; }
        public List<UserMessage> Messages { get; set; }
    }
}
