using System;

namespace BarberAPI.Models
{
    public class BarberPricesModel
    {
        public Guid BarberGd { get; set; }
        public double HPrice { get; set; }

        public double BPrice { get; set; }

        public double HBPrice { get; set; }
    }
}
