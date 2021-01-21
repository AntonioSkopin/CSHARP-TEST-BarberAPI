using AutoMapper;
using BarberAPI.DTO;
using BarberAPI.Entities;
using BarberAPI.Helpers;
using BarberAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarberAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private IClientService _clientService;
        private IMapper _mapper;

        public ClientController
        (
            IClientService clientService,
            IMapper mapper
        )
        {
            _clientService = clientService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<List<Appointment>> GetAppointments(Guid client_gd)
        {
            return Ok(_clientService.GetAppointments(client_gd));
        }

        [HttpGet]
        public async Task<ActionResult<BarberDTO>> GetBarberInfoOfAppointment(Guid appointment_gd)
        {
            return Ok(await _clientService.GetBarberInfoOfAppointment(appointment_gd));
        }

        [HttpPost]
        public async Task<ActionResult> PlanAppointment([FromBody] Appointment appointment)
        {
            if (ModelState.ErrorCount > 0)
            {
                throw new AppException("Please check your input and try again!");
            }

            await _clientService.PlanAppointment(appointment);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> CancelAppointment([FromBody] Guid appointment_gd)
        {
            if (ModelState.ErrorCount > 0)
            {
                throw new AppException("Please check your input and try again!");
            }

            await _clientService.CancelAppointment(appointment_gd);
            return Ok();
        }
    }
}
