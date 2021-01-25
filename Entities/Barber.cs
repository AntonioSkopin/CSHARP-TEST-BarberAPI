using System;
using System.ComponentModel.DataAnnotations;

namespace BarberAPI.Entities
{
    public class Barber
    {
        [Key]
        public Guid Gd { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public double HPrice { get; set; }

        public double BPrice { get; set; }

        public double HBPrice { get; set; }
    }
}
