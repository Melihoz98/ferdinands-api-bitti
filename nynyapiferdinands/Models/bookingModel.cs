using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography;

namespace nynyapiferdinands.Models
{
    public class Booking
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string postalCode { get; set; }
        public int antalPersoner { get; set; }
        public DateTime dato { get; set; }
        public TimeSpan tid { get; set; }
        public string ops { get; set; } 


        
    }
}
