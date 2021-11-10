using System.Collections.Generic;
using System.Threading.Tasks;
using GreenPoint.Dotnet.DataAccess.Models;
using GreenPoint.Dotnet.DataAccess.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace GreenPoint.Dotnet.WebAdmin.Pages
{
    [Authorize]
    public class SpotListPage : PageModel
    {
        private readonly SpotProvider _spotProvider;

        [BindProperty] public List<Spot> Spots { get; set; }

        public SpotListPage(SpotProvider spotProvider)
        {
            _spotProvider = spotProvider;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Spots = await _spotProvider.GetAll();
            return Page();
        }
    }
}