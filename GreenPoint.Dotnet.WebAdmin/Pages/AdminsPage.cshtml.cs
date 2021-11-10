using System.Collections.Generic;
using System.Threading.Tasks;
using GreenPoint.Dotnet.DataAccess.Models;
using GreenPoint.Dotnet.DataAccess.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GreenPoint.Dotnet.WebAdmin.Pages
{
    public class AdminsPage : PageModel
    {
        private readonly AdminProvider _adminProvider;

        [BindProperty] public List<Admin> Admins { get; set; }

        public AdminsPage(AdminProvider adminProvider)
        {
            _adminProvider = adminProvider;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Admins = await _adminProvider.GetAll();
            
            return Page();
        }
    }
}