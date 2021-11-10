using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GreenPoint.Dotnet.DataAccess.Models;
using GreenPoint.Dotnet.DataAccess.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GreenPoint.Dotnet.WebAdmin.Pages
{
    public class UserStatisticPage : PageModel
    {
        private readonly UserProvider _usersProvider;

        [BindProperty] public List<UserStatisticViewModel> Users { get; set; }

        public UserStatisticPage(UserProvider usersProvider)
        {
            _usersProvider = usersProvider;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Users = new List<UserStatisticViewModel>();
            var userList = await _usersProvider.GetAll();
            
            foreach (var user in userList)
            {
                Users.Add(new UserStatisticViewModel
                {
                    Email = user.Email,
                    Username = user.Username,
                    Rating = await _usersProvider.CountRating(user.Id),
                    IncreasedRating = await _usersProvider.CountRating(user.Id, DateTime.Today.AddDays(-7)),
                });
            }
            
            return Page();
        }
    }

    public class UserStatisticViewModel
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public int Rating { get; set; }
        public int IncreasedRating { get; set; }
    }
}