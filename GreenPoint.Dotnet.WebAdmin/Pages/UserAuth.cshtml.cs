using System;
using System.Net;
using System.Threading.Tasks;
using GreenPoint.Dotnet.Contracts.Dtos;
using GreenPoint.Dotnet.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace GreenPoint.Dotnet.WebAdmin.Pages
{
    public class UserAuth : PageModel
    {
        private readonly UserAuthenticationService _userAuthenticationService;
        private readonly Logger<UserAuth> _logger;

        [BindProperty]
        public SignInViewModel SignInViewModel { get; set; }

        [BindProperty]
        public SignUpViewModel SignUpViewModel { get; set; }

        public UserAuth(UserAuthenticationService userAuthenticationService)
        {
            _userAuthenticationService = userAuthenticationService;
        }
        
        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostSignIn()
        {
            var token = await _userAuthenticationService.Authenticate(SignInViewModel.Email, SignInViewModel.Password);
            
            Response.Cookies.Append("user-token", token);
            
            return RedirectToPage("CodeGenerationPage");
        }
        
        public async Task<IActionResult> OnPostSignUp()
        {
            var token = await _userAuthenticationService.Register(new UserRegistrationDto
            {
                Email = SignUpViewModel.Email,
                Password = SignUpViewModel.Password,
                Username = SignUpViewModel.Username
            });

            Response.Cookies.Append("user-token", token);
           
            return RedirectToPage("CodeGenerationPage");
        }
    }

    public class SignInViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    
    public class SignUpViewModel
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}