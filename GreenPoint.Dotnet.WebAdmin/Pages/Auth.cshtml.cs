using System;
using System.Threading.Tasks;
using GreenPoint.Dotnet.WebAdmin.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;

namespace GreenPoint.Dotnet.WebAdmin.Pages
{
    public class Auth : PageModel
    {
        private readonly AdminAuthenticationService _adminAuthentication;

        public Auth(AdminAuthenticationService adminAuthentication)
        {
            _adminAuthentication = adminAuthentication;
        }
        
        public void OnGet()
        {
            _adminAuthentication.SignOutUser();
        }

        public async Task<IActionResult> OnPostAsync([FromForm] string login, [FromForm] string password)
        {
            try
            {
                await _adminAuthentication.SignIn(login, password);
            }
            catch (ArgumentException e)
            {
                ViewData["ShowAlert"] = true;
                return Page();
            }
            
            return Redirect("/");
        } 
    }
}