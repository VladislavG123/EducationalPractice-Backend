using System;
using System.Threading.Tasks;
using GreenPoint.Dotnet.Contracts.Dtos;
using GreenPoint.Dotnet.Contracts.Parameters;
using GreenPoint.Dotnet.Contracts.ViewModels;
using GreenPoint.Dotnet.DataAccess.Models;
using GreenPoint.Dotnet.DataAccess.Providers;
using GreenPoint.Dotnet.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GreenPoint.Dotnet.WebApi.Controllers
{
    [Route("api/auth/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserAuthenticationService _authenticationService;

        public AuthenticationController(UserAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SignIn(LoginUserViewModel loginUserViewModel)
        {
            try
            {
                var token = await _authenticationService
                    .Authenticate(loginUserViewModel.Email, loginUserViewModel.Password);

                return token != null ? Ok(token) : Unauthorized();
            }
            catch (ArgumentException)
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        public async Task<IActionResult> SignUp(RegisterUserViewModel registerUser)
        {
            string token;
            try
            {
                token = await _authenticationService.Register(new UserRegistrationDto()
                {
                    Username = registerUser.Username,
                    Email = registerUser.Email,
                    Password = registerUser.Password
                });

            }
            catch (ArgumentException e)
            {
                return Unauthorized(e.Message);
            }

            return Ok(token);
        }

    }

}