using System.ComponentModel.DataAnnotations;

namespace BarberAPI.Models
{
    public class RegisterBarberModel
    {
        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public double HPrice { get; set; }

        [Required]
        public double BPrice { get; set; }

        [Required]
        public double HBPrice { get; set; }
    }
}
