using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using GreenPoint.Dotnet.DataAccess.Models;
using GreenPoint.Dotnet.DataAccess.Providers;
using GreenPoint.Dotnet.WebAdmin.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GreenPoint.Dotnet.WebAdmin.Pages
{
    public class AvatarPage : PageModel
    {
        private readonly AvatarProvider _avatarProvider;
        private readonly AwsS3FileUploadService _uploadService;
        

        [BindProperty] public List<Avatar> Avatars { get; set; }

        public AvatarPage(AvatarProvider avatarProvider, AwsS3FileUploadService uploadService)
        {
            _avatarProvider = avatarProvider;
            _uploadService = uploadService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Avatars = await _avatarProvider.GetAll();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync([Required][FromForm] IFormFile formFile)
        {   
            await using var memoryStream = new MemoryStream();

            await formFile.CopyToAsync(memoryStream);

            // Upload the file if less than 2 MB
            if (memoryStream.Length < 2097152)
            {
                var url = await _uploadService
                    .UploadFileAsync(memoryStream, formFile.FileName, formFile.ContentType);
                if (url is not null)
                {
                    await _avatarProvider.Add(new Avatar
                    {
                        Url = url
                    });
                }
            }
            else
            {
                ViewData["Error"] = "Файл превышает 2МБ";
            }

            return await OnGetAsync();
        }
    }
}