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
    public interface IClientService
    {
        List<Appointment> GetAppointments(Guid client_gd);
        Task CancelAppointment(Guid appointment_gd);
        Task PlanAppointment(Appointment appointment);
        Task<BarberDTO> GetBarberInfoOfAppointment(Guid appointment_gd);
        Task<AppointmentDTO> GetUpcommingAppointment(Guid user_gd);
    }

    public class ClientService : SqlService, IClientService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public ClientService(IConfiguration configuration, DataContext context) : base(configuration)
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

        public async Task<AppointmentDTO> GetUpcommingAppointment(Guid user_gd)
        {
            var getUpcommingAppointmentQuery =
            @"
                SELECT apmnt.*, shop.Location, shop.Shopname, barber.Phone, barber.Username FROM Appointments apmnt
                LEFT JOIN BarbershopBinds bind ON apmnt.BarberGd = bind.BarberGd
                LEFT JOIN Barbershops shop ON bind.BarbershopGd = shop.Gd
                LEFT JOIN Barbers barber ON apmnt.BarberGd = barber.Gd
                WHERE apmnt.ClientGd = @_user_gd
            ";

            return await GetQuery<AppointmentDTO>(getUpcommingAppointmentQuery, new
            {
                _user_gd = user_gd
            });
        }

        public List<Appointment> GetAppointments(Guid client_gd)
        {
            return _context.Appointments.Where(x => x.ClientGd == client_gd).ToList();
        }

        public async Task<BarberDTO> GetBarberInfoOfAppointment(Guid appointment_gd)
        {
            var getBarberInfoQuery =
            @"
                SELECT * FROM Barbers barber
                LEFT JOIN Appointments apmnt on barber.Gd = apmnt.BarberGd
                WHERE apmnt.Gd = @_appointment_gd
            ";

            return await GetQuery<BarberDTO>(getBarberInfoQuery, new
            {
                _appointment_gd = appointment_gd
            });
        }

        public async Task PlanAppointment(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
        }
    }
}
