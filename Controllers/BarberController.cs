using AutoMapper;
using BarberAPI.Entities;
using BarberAPI.Helpers;
using BarberAPI.Models;
using BarberAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarberAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BarberController : ControllerBase
    {
        private IBarberService _barberService;
        private IMapper _mapper;

        public BarberController
        (
            IBarberService barberService,
            IMapper mapper
        )
        {
            _barberService = barberService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<List<Appointment>> GetAppointments(Guid barber_gd)
        {
            return Ok(_barberService.GetAppointments(barber_gd));
        }

        [HttpGet]
        public async Task<ActionResult<List<Appointment>>> GetTodayAppointments(Guid barber_gd)
        {
            return Ok(await _barberService.GetTodayAppointments(barber_gd));
        }

        [HttpGet]
        public async Task<ActionResult<double>> GetDayIncome(Guid barber_gd)
        {
            return Ok(await _barberService.GetDayIncome(barber_gd));
        }

        [HttpGet]
        public async Task<ActionResult<double>> GetMonthIncome(Guid barber_gd)
        {
            return Ok(await _barberService.GetMonthIncome(barber_gd));
        }

        [HttpGet]
        public async Task<ActionResult<double>> GetYearIncome(Guid barber_gd)
        {
            return Ok(await _barberService.GetYearIncome(barber_gd));
        }

        [HttpPost]
        public async Task<ActionResult> PlanAppointment([FromBody] Appointment appointment)
        {
            if (ModelState.ErrorCount > 0)
            {
                throw new AppException("Please check your input and try again!");
            }

            await _barberService.PlanAppointment(appointment);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> CancelAppointment([FromBody] Guid appointment_gd)
        {
            if (ModelState.ErrorCount > 0)
            {
                throw new AppException("Please check your input and try again!");
            }

            await _barberService.CancelAppointment(appointment_gd);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdatePrices([FromBody] BarberPricesModel model)
        {
            if (ModelState.ErrorCount > 0)
            {
                throw new AppException("Please check your input and try again!");
            }

            await _barberService.UpdatePrices(model);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAppointment([FromBody] Appointment model)
        {
            await _barberService.UpdateAppointment(model);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAccount(Guid barber_gd)
        {
            await _barberService.DeleteAccount(barber_gd);
            return Ok();
        }
    }
}