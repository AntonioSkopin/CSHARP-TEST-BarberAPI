using System;
using System.ComponentModel.DataAnnotations;

namespace BarberAPI.Entities
{
    public class Barbershop
    {
        [Key]
        public Guid Gd { get; set; }

        public string Shopname { get; set; }

        public Guid OwnerGd { get; set; }

        public string Phone { get; set; }

        public string Location { get; set; }

        public DateTime DateOpened { get; set; }
    }
}
