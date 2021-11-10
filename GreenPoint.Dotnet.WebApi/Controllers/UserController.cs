using System;
using System.Threading.Tasks;
using GreenPoint.Dotnet.Contracts.Parameters;
using GreenPoint.Dotnet.Contracts.ViewModels;
using GreenPoint.Dotnet.DataAccess.Models;
using GreenPoint.Dotnet.DataAccess.Providers;
using GreenPoint.Dotnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace GreenPoint.Dotnet.WebApi.Controllers
{
    [Authorize]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly UserProvider _userProvider;
        private readonly AvatarProvider _avatarProvider;
        private readonly StatusProvider _statusProvider;
        private readonly UserCodeProvider _userCodeProvider;
        private readonly UserAuthenticationService _authenticationService;

        public UserController(UserProvider userProvider, UserAuthenticationService authenticationService, 
            AvatarProvider avatarProvider, StatusProvider statusProvider, UserCodeProvider userCodeProvider)
        {
            _userProvider = userProvider;
            _authenticationService = authenticationService;
            _avatarProvider = avatarProvider;
            _statusProvider = statusProvider;
            _userCodeProvider = userCodeProvider;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _authenticationService
                .GetUserByHeaders(Request.Headers[HeaderNames.Authorization].ToArray());
            var avatar = await _avatarProvider.GetById(user.AvatarId);

            var userRating = await _userProvider.CountRating(user.Id);
            
            var status = await _statusProvider.GetByRating(userRating);
            
            return Ok(new UserViewModel
            {
                Email = user.Email,
                Id = user.Id,
                Username = user.Username,
                Status = status.Name,
                AvatarUrl = avatar?.Url
            });

        }
        
        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody] PatchUserParameter parameter)
        {
            var user = await _authenticationService
                .GetUserByHeaders(Request.Headers[HeaderNames.Authorization].ToArray());
            
            if(!string.IsNullOrEmpty(parameter.Username))
            {
                user.Username = parameter.Username;
            }
            
            if(parameter.AvatarId != Guid.Empty)
            {
                user.AvatarId = parameter.AvatarId;
            }
            
            if(!string.IsNullOrEmpty(parameter.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(parameter.Password);
            }
            
            await _userProvider.Edit(user);
            return NoContent();
        }

        [HttpGet("code")]
        [Authorize]
        public async Task<IActionResult> GetCode()
        {
            var user = await _authenticationService
                .GetUserByHeaders(Request.Headers[HeaderNames.Authorization].ToArray());

            var random = new Random();
            var code = $"{random.Next(1000, 9999)}-{random.Next(1000, 9999)}";

            var oldOne = await _userCodeProvider.FirstOrDefault(x => x.UserId.Equals(user.Id));

            if (oldOne != null)
            {
                await _userCodeProvider.Remove(oldOne);
            }

            await _userCodeProvider.Add(new UserCode
            {
                Id = Guid.NewGuid(),
                Code = code,
                UserId = user.Id
            });

            return Ok(code);
        }

        [HttpPost("code/{code}")]
        public async Task<IActionResult> GetTokenByCode(string code)
        {
            var userCode = await _userCodeProvider.FirstOrDefault(x => x.Code.Equals(code));
            
            if (userCode is null)
            {
                return BadRequest();
            }

            await _userCodeProvider.Remove(userCode);
            
            var user = await _userProvider.FirstOrDefault(x => x.Id.Equals(userCode.UserId));

            if (user is null)
            {
                return BadRequest();
            }

            return Ok(_authenticationService.GenerateJwtToken(user.Email));
        }
    }
}