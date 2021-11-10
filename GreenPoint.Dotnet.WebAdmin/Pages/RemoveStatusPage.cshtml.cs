using System;
using System.Threading.Tasks;
using GreenPoint.Dotnet.DataAccess.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GreenPoint.Dotnet.WebAdmin.Pages
{
    public class RemoveStatusPage : PageModel
    {
        private readonly StatusProvider _statusProvider;

        public RemoveStatusPage(StatusProvider statusProvider)
        {
            _statusProvider = statusProvider;
        }
        
        public async Task<IActionResult> OnGetAsync()
        {
            var id = Guid.Parse(RouteData.Values["id"] as string ?? string.Empty );
            
            await _statusProvider.Remove(await _statusProvider.GetById(id));

            return Redirect("/statuses");
        }
    }
}