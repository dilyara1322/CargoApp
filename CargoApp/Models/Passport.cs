using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CargoApp.Models
{
    public class Passport
    {
        [Required]
        [Key]
        public int ClientId { get; set; }
        public Client Client { get; set; }
        [Required]
        //[MinLength(4)]
        //[MaxLength(4)]
      //  [Column(TypeName = "varchar(4)")]
        public string Series { get; set; }
        [Required]
        //[MinLength(6)]
        //[MaxLength(6)]
       // [Column(TypeName = "varchar(6)")]
        public string Number { get; set; }

        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public string Sex { get; set; }
        public DateTime? BirthDate { get; set; }
        // public Address BirthPlace { get; set; }
        public string BirthPlace { get; set; }

        public string IssuedBy { get; set; }
        public DateTime? IssuedDate { get; set; }

        [MaxLength(7)]
        public string Code { get; set; }
    }
}
