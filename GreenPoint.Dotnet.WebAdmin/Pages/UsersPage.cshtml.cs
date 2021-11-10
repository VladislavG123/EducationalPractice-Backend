using System.Collections.Generic;
using System.Threading.Tasks;
using GreenPoint.Dotnet.DataAccess.Models;
using GreenPoint.Dotnet.DataAccess.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GreenPoint.Dotnet.WebAdmin.Pages
{
    public class UsersPage : PageModel
    { 
        private readonly UserProvider _usersProvider;
        private readonly AvatarProvider _avatarProvider;

        [BindProperty] public List<User> Users { get; set; }
        [BindProperty] public List<Avatar> Avatars { get; set; }

        public UsersPage(UserProvider usersProvider, AvatarProvider avatarProvider)
        {
            _usersProvider = usersProvider;
            _avatarProvider = avatarProvider;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Users = await _usersProvider.GetAll();
            Avatars = await _avatarProvider.GetAll();
            
            return Page();
        }

    }
}