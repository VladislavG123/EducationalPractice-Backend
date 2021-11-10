using System.Collections.Generic;
using System.Threading.Tasks;
using GreenPoint.Dotnet.DataAccess.Models;
using GreenPoint.Dotnet.DataAccess.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GreenPoint.Dotnet.WebAdmin.Pages
{
    public class CommentsPage : PageModel
    {
        private readonly CommentProvider _commentProvider;

        [BindProperty] public List<Comment> Comments { get; set; }
        
        public CommentsPage(CommentProvider commentProvider)
        {
            _commentProvider = commentProvider;
        }
        
        public async Task<IActionResult> OnGetAsync()
        {
            Comments = await _commentProvider.GetAll();

            return Page();
        }
    }
}