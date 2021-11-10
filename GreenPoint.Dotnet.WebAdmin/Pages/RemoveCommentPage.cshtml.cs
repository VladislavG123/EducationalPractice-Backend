using System;
using System.Threading.Tasks;
using GreenPoint.Dotnet.DataAccess.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GreenPoint.Dotnet.WebAdmin.Pages
{
    public class RemoveCommentPage : PageModel
    {
        private readonly CommentProvider _commentsProvider;

        public RemoveCommentPage(CommentProvider commentsProvider)
        {
            _commentsProvider = commentsProvider;
        }
        
        public async Task<IActionResult> OnGetAsync()
        {
            var id = Guid.Parse(RouteData.Values["id"] as string ?? string.Empty);

            await _commentsProvider.Remove(await _commentsProvider.GetById(id));

            return Redirect("/comments");
        }
    }
}