using BarberAPI.Entities;
using BarberAPI.Helpers;
using BarberAPI.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarberAPI.Services
{
    public interface IBarberService
    {
        List<Appointment> GetAppointments(Guid barber_gd);
        Task<List<Appointment>> GetTodayAppointments(Guid barber_gd);
        Task CancelAppointment(Guid appointment_gd);
        Task UpdateAppointment(Appointment model);
        Task PlanAppointment(Appointment appointment);
        Task UpdatePrices(BarberPricesModel model);
        Task<double> GetDayIncome(Guid barber_gd);
        Task<double> GetMonthIncome(Guid barber_gd);
        Task<double> GetYearIncome(Guid barber_gd);
        Task DeleteAccount(Guid barber_gd);
    }

    public class BarberService : SqlService, IBarberService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public BarberService(IConfiguration configuration, DataContext context) : base(configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task CancelAppointment(Guid appointment_gd)
        {
            // Query to set an appointment to canceled
            var cancelAppointmentQuery =
            @"
                UPDATE Appointments
                SET isCanceled = 1
                WHERE Gd = @_appointment_gd
            ";

            await PutQuery(cancelAppointmentQuery, new
            {
                _appointment_gd = appointment_gd
            });
        }

        public List<Appointment> GetAppointments(Guid barber_gd)
        {
            return _context.Appointments.Where(x => x.BarberGd == barber_gd).ToList();
        }

        public async Task<List<Appointment>> GetTodayAppointments(Guid barber_gd)
        {
            // FIX QUERY:
            var getTodayAppointments =
            @"
                SELECT * FROM Appointments
                WHERE BarberGd = @_barber_gd
                AND Date = cast(getdate() as date)
            ";

            return await GetManyQuery<Appointment>(getTodayAppointments, new
            {
                _barber_gd = barber_gd
            });
        }

        public async Task<double> GetDayIncome(Guid barber_gd)
        {
            var getDayIncomeQuery =
            @"
                SELECT SUM(Price) FROM Appointments
                WHERE BarberGd = @_barber_gd
                AND CAST(apnmt.Date AS Date) = CASTE(GETDATE() AS Date)
            ";

            return await GetQuery<double>(getDayIncomeQuery, new
            {
                _barber_gd = barber_gd
            });
        }

        public async Task<double> GetMonthIncome(Guid barber_gd)
        {
            var getMonthIncomeQuery =
            @"
                SELECT SUM(Price) FROM Appointments
                WHERE BarberGd = @_barber_gd
                AND Month(Date) = Month(GETDATE())
            ";

            return await GetQuery<double>(getMonthIncomeQuery, new
            {
                _barber_gd = barber_gd
            });
        }

        public async Task<double> GetYearIncome(Guid barber_gd)
        {
            var getYearIncomeQuery =
            @"
                SELECT SUM(Price) FROM Appointments
                WHERE BarberGd = @_barber_gd
                AND YEAR(Date) = YEAR(GETDATE())
            ";

            return await GetQuery<double>(getYearIncomeQuery, new
            {
                _barber_gd = barber_gd
            });
        }

        public async Task PlanAppointment(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAppointment(Appointment model)
        {
            var appointment = await _context.Appointments.FindAsync(model);

            if (appointment == null)
                throw new AppException("Appointment not found");

            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePrices(BarberPricesModel model)
        {
            var updatePricesQuery =
            @"
                UPDATE Barbers
                SET HPrice = @_h_price,
                    BPrice = @_b_price,
                    HBPrice = @_hb_price
                WHERE Gd = @_barber_gd
            ";

            await PutQuery(updatePricesQuery, new
            {
                _h_price = model.HPrice,
                _b_price = model.BPrice,
                _hb_price = model.HBPrice,
                _barber_gd = model.BarberGd
            });
        }

        public async Task DeleteAccount(Guid barber_gd)
        {
            var barber = await _context.Barbers.FindAsync(barber_gd);
            if (barber != null)
            {
                _context.Barbers.Remove(barber);
                await _context.SaveChangesAsync();
            }
        }
    }
}
