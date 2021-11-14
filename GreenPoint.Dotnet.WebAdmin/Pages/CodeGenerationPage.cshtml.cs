using System;
using System.Threading.Tasks;
using GreenPoint.Dotnet.DataAccess.Models;
using GreenPoint.Dotnet.DataAccess.Providers;
using GreenPoint.Dotnet.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GreenPoint.Dotnet.WebAdmin.Pages
{
    public class CodeGenerationPage : PageModel
    {
        public string Code { get; set; }
        
        private readonly UserAuthenticationService _userAuthenticationService;
        private readonly UserCodeProvider _userCodeProvider;
        private readonly UserProvider _userProvider;

        public CodeGenerationPage(UserAuthenticationService userAuthenticationService, 
            UserCodeProvider userCodeProvider, UserProvider userProvider)
        {
            _userAuthenticationService = userAuthenticationService;
            _userCodeProvider = userCodeProvider;
            _userProvider = userProvider;
        }
        
        public async Task<IActionResult> OnGet()
        {
            if (Request.Cookies.TryGetValue("user-token", out string token))
            {
                var claims = _userAuthenticationService.DecryptToken(token);

                var user = await _userProvider.GetByEmailOrPhone(claims.Email);

                if (user is null)
                {
                    return Forbid();
                }
                
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

                Code = code;
                
                return Page();
            }
            else
            {
                return Forbid();
            }
        }
    }
}