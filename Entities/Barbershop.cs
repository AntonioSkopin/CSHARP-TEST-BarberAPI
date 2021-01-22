using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BarberAPI.Entities
{
    public class Barbershop
    {
        [Key]
        public Guid Gd { get; set; }

        public string Shopname { get; set; }

        public Guid OwnerGd { get; set; }

        public DateTime DateOpened { get; set; }
    }
}
