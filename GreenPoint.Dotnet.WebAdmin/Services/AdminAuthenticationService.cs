using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GreenPoint.Dotnet.DataAccess.Models;
using GreenPoint.Dotnet.DataAccess.Providers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GreenPoint.Dotnet.WebAdmin.Services
{
    public class AdminAuthenticationService
    {
        private readonly AdminProvider _adminProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminAuthenticationService(IHttpContextAccessor httpContext, AdminProvider adminProvider)
        {
            this._httpContextAccessor = httpContext;
            _adminProvider = adminProvider;
        }

        public async Task SignIn(string login, string password)
        {
            var user = await _adminProvider.GetByLogin(login);

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                throw new ArgumentException("Incorrect password");

            await WriteCookie(user.Login);
        }

        private async Task WriteCookie(string login)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, login)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = false,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),
                RedirectUri = "/"
            };

            await _httpContextAccessor.HttpContext.SignInAsync(
                Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        public async void SignOutUser()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(
                Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private string DecryptClaim()
        {
            // Get the encrypted cookie value
            var opt = _httpContextAccessor.HttpContext.RequestServices
                .GetRequiredService<
                    IOptionsMonitor<Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions>>();

            var cookie = opt.CurrentValue.CookieManager
                .GetRequestCookie(_httpContextAccessor.HttpContext, ".AspNetCore.Cookies");

            // Decrypt if found
            if (string.IsNullOrEmpty(cookie)) return null;
            var dataProtector = opt.CurrentValue.DataProtectionProvider
                .CreateProtector("Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationMiddleware",
                    Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme,
                    "v2");

            var ticketDataFormat = new TicketDataFormat(dataProtector);
            var ticket = ticketDataFormat.Unprotect(cookie);
            var claims = ticket.Principal.Claims;
            var list = claims.ToList();

            return list[0].Value;
        }

        public async Task<Admin> GetAdmin()
        {
            var login = DecryptClaim();

            return await _adminProvider.GetByLogin(login);
        }
    }
}