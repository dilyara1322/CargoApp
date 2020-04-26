using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CargoApp.Models
{
    public class UserMessage
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int RequestId { get; set; }
        public Request Request { get; set; }
        [Required]
        public int ClientId { get; set; }
        public Client Client { get; set; }
        [Required]
        public int LogisticianId { get; set; }
        public Logistician Logistician { get; set; }
        [Required]
        public bool ClientIsAuthor { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public DateTime DateTime { get; set; }

    }
}
