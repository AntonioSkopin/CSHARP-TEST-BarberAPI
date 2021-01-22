using BarberAPI.DTO;
using BarberAPI.Entities;
using BarberAPI.Helpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarberAPI.Services
{
    public interface IBarbershopOwnerService
    {
        Task AddBarberToShop(Guid barbershop_gd, Guid barber_gd);
        Task<List<BarberDTO>> GetEmployees(Guid owner_gd);
        Task<double> GetDayIncome(Guid owner_gd);
        Task<double> GetMonthIncome(Guid owner_gd);
        Task<double> GetYearIncome(Guid owner_gd);
        Task FireEmployee(Guid barber_gd);
        Task UpdateEmployee(Barber barber);
        Task<List<Appointment>> GetAllAppointments(Guid shop_gd);
    }

    public class BarbershopOwnerService : SqlService, IBarbershopOwnerService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public BarbershopOwnerService(IConfiguration configuration, DataContext context) : base(configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task AddBarberToShop(Guid barbershop_gd, Guid barber_gd)
        {
            var addBarberToShopQuery =
            @"
                INSERT INTO BarbershopBinds
                VALUES
                (
                    NEWID(), @_barbershop_gd, @_barber_gd, GETDATE()
                )
            ";

            await PostQuery(addBarberToShopQuery, new
            {
                _barbershop_gd = barbershop_gd,
                _barber_gd = barber_gd
            });
        }

        public async Task FireEmployee(Guid barber_gd)
        {
            var firedEmployee = _context.BarbershopBinds.Where(x => x.BarberGd == barber_gd).FirstOrDefault();
            if (firedEmployee == null)
                throw new AppException("Employee not found!");

            _context.BarbershopBinds.Remove(firedEmployee);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Appointment>> GetAllAppointments(Guid shop_gd)
        {
            var getAllAppointmentsQuery =
            @"
                SELECT * FROM Appointments apnmt
                LEFT JOIN BarbershopBinds bind ON apnmt.BarberGd = bind.BarberGd
                WHERE BarbershopGd = @_shop_gd
            ";

            return await GetManyQuery<Appointment>(getAllAppointmentsQuery, new
            {
                _shop_gd = shop_gd
            });
        }

        public async Task<List<BarberDTO>> GetEmployees(Guid owner_gd)
        {
            var getEmployeesQuery =
            @"
                SELECT barber.* FROM Barbers barber
                LEFT JOIN BarbershopBinds bind on bind.BarberGd = barber.Gd
                LEFT JOIN Barbershops shop ON shop.Gd = bind.BarbershopGd
                WHERE shop.OwnerGd = @_owner_gd
            ";

            return await GetManyQuery<BarberDTO>(getEmployeesQuery, new
            {
                _owner_gd = owner_gd
            });
        }

        public async Task<double> GetDayIncome(Guid owner_gd)
        {
            var getDayIncomeQuery =
            @"
                SELECT SUM(apnmt.Price) FROM Appointments apnmt
                LEFT JOIN BarbershopBinds bind ON apnmt.BarberGd = bind.BarberGd
                LEFT JOIN Barbershops shop ON bind.BarbershopGd = shop.Gd
                WHERE shop.OwnerGd = @_owner_gd
                AND CAST(apnmt.Date AS Date) = CASTE(GETDATE() AS Date)
            ";

            return await GetQuery<double>(getDayIncomeQuery, new
            {
                _owner_gd = owner_gd
            });
        }

        public async Task<double> GetMonthIncome(Guid owner_gd)
        {
            var getMonthIncomeQuery =
            @"
                SELECT SUM(apnmt.Price) FROM Appointments apnmt
                LEFT JOIN BarbershopBinds bind ON apnmt.BarberGd = bind.BarberGd
                LEFT JOIN Barbershops shop ON bind.BarbershopGd = shop.Gd
                WHERE shop.OwnerGd = @_owner_gd
                AND Month(Date) = Month(GETDATE())
            ";

            return await GetQuery<double>(getMonthIncomeQuery, new
            {
                _owner_gd = owner_gd
            });
        }

        public async Task<double> GetYearIncome(Guid owner_gd)
        {
            var getYearIncomeQuery =
            @"
                SELECT SUM(apnmt.Price) FROM Appointments apnmt
                LEFT JOIN BarbershopBinds bind ON apnmt.BarberGd = bind.BarberGd
                LEFT JOIN Barbershops shop ON bind.BarbershopGd = shop.Gd
                WHERE shop.OwnerGd = @_owner_gd
                AND YEAR(Date) = YEAR(GETDATE())
            ";

            return await GetQuery<double>(getYearIncomeQuery, new
            {
                _owner_gd = owner_gd
            });
        }

        public async Task UpdateEmployee(Barber barberParam)
        {
            var barber = await _context.Barbers.FindAsync(barberParam);

            if (barber == null)
                throw new AppException("Barber not found");

            _context.Barbers.Update(barber);
            await _context.SaveChangesAsync();
        }
    }
}
