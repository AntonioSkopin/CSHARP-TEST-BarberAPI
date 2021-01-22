using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BarberAPI.Entities
{
    public class BarbershopBind
    {
        [Key]
        public Guid Gd { get; set; }

        public Guid BarbershopGd { get; set; }

        public Guid BarberGd { get; set; }

        public DateTime DateJoined { get; set; }
    }
}