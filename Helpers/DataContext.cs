using BarberAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BarberAPI.Helpers
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration _configuration;

        public DataContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // Connect to SQL server database
            options.UseSqlServer(_configuration.GetConnectionString("BarbershopDB"));
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Barber> Barbers { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<BarbershopOwner> BarbershopOwners { get; set; }
        public DbSet<Barbershop> Barbershops { get; set; }
        public DbSet<BarbershopBind> BarbershopBinds { get; set; }
    }
}
