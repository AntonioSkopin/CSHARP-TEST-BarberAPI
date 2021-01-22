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
    public class BarbershopOwnerController : ControllerBase
    {
        private IBarbershopOwnerService _barbershopOwnerService;
        private IMapper _mapper;

        public BarbershopOwnerController
        (
            IBarbershopOwnerService barbershopOwnerService,
            IMapper mapper
        )
        {
            _barbershopOwnerService = barbershopOwnerService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Appointment>>> GetAllAppointments(Guid shop_gd)
        {
            return Ok(await _barbershopOwnerService.GetAllAppointments(shop_gd));
        }

        [HttpGet]
        public async Task<ActionResult<List<BarberDTO>>> GetEmployees(Guid owner_gd)
        {
            return Ok(await _barbershopOwnerService.GetEmployees(owner_gd));
        }

        [HttpGet]
        public async Task<ActionResult<double>> GetDayIncome(Guid owner_gd)
        {
            return Ok(await _barbershopOwnerService.GetDayIncome(owner_gd));
        }

        [HttpGet]
        public async Task<ActionResult<double>> GetMonthIncome(Guid owner_gd)
        {
            return Ok(await _barbershopOwnerService.GetMonthIncome(owner_gd));
        }

        [HttpGet]
        public async Task<ActionResult<double>> GetYearIncome(Guid owner_gd)
        {
            return Ok(await _barbershopOwnerService.GetYearIncome(owner_gd));
        }

        [HttpPost]
        public async Task<ActionResult> AddBarberToShop([FromBody] Guid barbershop_gd, Guid barber_gd)
        {
            if (ModelState.ErrorCount > 0)
            {
                throw new AppException("Please check your input and try again!");
            }

            await _barbershopOwnerService.AddBarberToShop(barbershop_gd, barber_gd);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateEmployee([FromBody] Barber barber)
        {
            if (ModelState.ErrorCount > 0)
            {
                throw new AppException("Please check your input and try again!");
            }

            await _barbershopOwnerService.UpdateEmployee(barber);
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> FireEmployee(Guid barber_gd)
        {
            await _barbershopOwnerService.FireEmployee(barber_gd);
            return Ok();
        }
    }
}
