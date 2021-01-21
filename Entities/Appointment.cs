using System;
using System.ComponentModel.DataAnnotations;

namespace BarberAPI.Entities
{
    public class Appointment
    {
        [Key]
        public Guid Gd { get; set; }

        public Guid BarberGd { get; set; }

        public Guid ClientGd { get; set; }

        public DateTime Date { get; set; }

        public double Price { get; set; }

        public bool IsCanceled { get; set; }
    }
}
