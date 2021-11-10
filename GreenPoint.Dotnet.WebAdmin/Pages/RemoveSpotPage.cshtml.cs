using System;
using System.Threading.Tasks;
using GreenPoint.Dotnet.DataAccess.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GreenPoint.Dotnet.WebAdmin.Pages
{
    [Authorize]
    public class RemoveSpotPage : PageModel
    {
        private readonly SpotProvider _spotProvider;

        public RemoveSpotPage(SpotProvider spotProvider)
        {
            _spotProvider = spotProvider;
        }
        
        public async Task<IActionResult> OnGetAsync()
        {
            var id = Guid.Parse(RouteData.Values["id"] as string ?? string.Empty );
            
            await _spotProvider.Remove(await _spotProvider.GetById(id));

            return Redirect("/spots");
        }
    }
}