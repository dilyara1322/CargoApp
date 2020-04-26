using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CargoApp.Models
{
    public class Rating
    {
       // public int Id { get; set; }

        [Required]
        public int CompanyId { get; set; }
     //   public Company Company { get; set; }

        [Required]
        public int ClientId { get; set; }
        //  public Client Client { get; set; }
        public int MarkFromUserToConpany { get; set; }
        public int MarkFromCompanyToUser { get; set; }
    }
}
