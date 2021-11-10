using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenPoint.Dotnet.DataAccess.Models;
using GreenPoint.Dotnet.DataAccess.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace GreenPoint.Dotnet.WebAdmin.Pages
{
    public class SpotMapPage : PageModel
    {
        private readonly SpotProvider _spotProvider;

        [BindProperty] public string SpotsJson { get; set; }

        public SpotMapPage(SpotProvider spotProvider)
        {
            _spotProvider = spotProvider;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var spots = await _spotProvider.GetAll();
            
            SpotsJson = JsonConvert.SerializeObject(spots.Select(x => 
                new
                {
                    x.Langitude,
                    x.Latitude,
                    x.Title
                }).ToArray());
            
            return Page();
        }
    }
}