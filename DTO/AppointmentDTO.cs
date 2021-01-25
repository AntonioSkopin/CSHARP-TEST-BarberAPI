using System;

namespace BarberAPI.DTO
{
    public class AppointmentDTO
    {
        public Guid Gd { get; set; }

        public string Username { get; set; }

        public string Shopname { get; set; }

        public string Location { get; set; }

        public string Phone { get; set; }

        public double Price { get; set; }
    }
}
