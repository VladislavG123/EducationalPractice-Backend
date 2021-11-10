using System;
using System.Threading.Tasks;
using GreenPoint.Dotnet.DataAccess.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GreenPoint.Dotnet.WebAdmin.Pages
{
    public class RemoveAvatarPage : PageModel
    {
        private readonly AvatarProvider _avatarProvider;

        public RemoveAvatarPage(AvatarProvider avatarProvider)
        {
            _avatarProvider = avatarProvider;
        }
        
        public async Task<IActionResult> OnGetAsync()
        {
            var id = Guid.Parse(RouteData.Values["id"] as string ?? string.Empty );
            
            await _avatarProvider.Remove(await _avatarProvider.GetById(id));

            return Redirect("/avatars");
        }
    }
}