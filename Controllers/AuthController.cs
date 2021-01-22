using BarberAPI.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BarberAPI.Services;
using BarberAPI.Models;
using BarberAPI.Entities;

namespace BarberAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public AuthController
        (
            IAuthService authService,
            IMapper mapper,
            IOptions<AppSettings> appSettings
        )
        {
            _authService = authService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Client>> GetByGd(Guid gd)
        {
            var user = await _authService.GetByGd(gd);
            var model = _mapper.Map<ClientModel>(user);
            return Ok(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            var user = _authService.Authenticate(model.Username, model.Password);

            if (user == null)
                return Unauthorized(new { message = "Username or password is incorrect" });

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        // User.Values holds GD of user
                        new Claim(ClaimTypes.Name, user.Values.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                // Return basic user info and authentication token
                return Ok(new
                {
                    TypeUser = user.Keys,
                    Gd = user.Values,
                    Token = tokenString
                });
            }
            catch (AppException ex)
            {
                // Return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Client>> RegisterClient([FromBody] RegisterModel model)
        {
            // Map model to entity
            var user = _mapper.Map<Client>(model);

            try
            {
                // Create user
                await _authService.RegisterClient(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // Return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Barber>> RegisterBarber([FromBody] RegisterBarberModel model)
        {
            // Map model to entity
            var user = _mapper.Map<Barber>(model);

            try
            {
                // Create user
                await _authService.RegisterBarber(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // Return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<BarbershopOwner>> RegisterBarbershopOwner([FromBody] RegisterModel model)
        {
            // Map model to entity
            var user = _mapper.Map<BarbershopOwner>(model);

            try
            {
                // Create user
                await _authService.RegisterBarbershopOwner(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // Return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateClient(Guid gd, [FromBody] UpdateModel model)
        {
            // Map model to entity and set id
            var user = _mapper.Map<Client>(model);
            user.Gd = gd;

            try
            {
                // Update user 
                await _authService.UpdateClient(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // Return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteClientAccount(Guid client_gd)
        {
            await _authService.DeleteClientAccount(client_gd);
            return Ok();
        }
    }
}
