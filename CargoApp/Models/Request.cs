﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CargoApp.Models
{
    public class Request
    {
        [Required]
        public int Id { get; set; }

        //[Required]
        public int? ClientId { get; set; }
        public Client Client { get; set; }

       // [Required]
        public int? CompanyId { get; set; }
        public Company Company { get; set; }
        public int? DriverId { get; set; }
        public Driver Driver { get; set; }

        public enum Status { Free, Accepted, DriverIsAssigned, OnTheWay, Arrived, Received } //подумать
        [Required]
        public Status? CurrentStatus { get; set; } = Status.Free;

        //public Address CurrentLocation { get; set; } //latitude + longitude?
        public float? CurrentLatitude { get; set; } // умноженное на 1 000 000
        public float? CurrentLongitude { get; set; } // умноженное на 1 000 000

        /*
        [NotMapped]
        public float RealLatitude { get; set; } //реальное значение, не хранится в базе
        [NotMapped]
        public float RealLongitude { get; set; } //реальное значение, не хранится в базе
        */

        public Address SendingAddress { get; set; }
        public DateTime? SendingDateTime { get; set; }
        public Address ReceivingAddress { get; set; }
        public DateTime? ReceivingDateTime { get; set; }
        public string Addition { get; set; }

        public List<Good> Goods { get; set; }
        public List<UserMessage> Messages { get; set; }
    }
}
