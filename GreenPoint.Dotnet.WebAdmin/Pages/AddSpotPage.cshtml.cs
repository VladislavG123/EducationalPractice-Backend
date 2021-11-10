using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GreenPoint.Dotnet.DataAccess.Models;
using GreenPoint.Dotnet.DataAccess.Providers;
using GreenPoint.Dotnet.WebAdmin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Net.Http.Headers;

namespace GreenPoint.Dotnet.WebAdmin.Pages
{
    [Authorize]
    public class AddSpotPage : PageModel
    {
        private readonly SpotProvider _spotProvider;
        private readonly AwsS3FileUploadService _uploadService;
        private readonly SpotImageProvider _imageProvider;

        public AddSpotPage(SpotProvider spotProvider, AwsS3FileUploadService uploadService,
            SpotImageProvider imageProvider)
        {
            _spotProvider = spotProvider;
            _uploadService = uploadService;
            _imageProvider = imageProvider;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostSpot(
            [FromForm] string title,
            [FromForm] string details,
            [FromForm] string lat,
            [FromForm] string lng,
            [FromForm] string urls)
        {

            if (!double.TryParse(lat, out double latD))
            {
                latD = double.Parse(lat.Replace('.', ','));
            }
            
            if (!double.TryParse(lng, out double lngD))
            {
                lngD = double.Parse(lng.Replace('.', ','));
            }
            
            try
            {
                var spot = new Spot
                {
                    Details = details,
                    Langitude = lngD,
                    Latitude = latD,
                    Title = title
                };
                
                await _spotProvider.Add(spot);
                
                
                var urlList = urls.Trim(',').Split(',').ToList();

                foreach (var url in urlList)
                {
                    await _imageProvider.Add(new SpotImage
                    {
                        Url = url,
                        SpotId = spot.Id
                    });
                }
                
            }
            catch (Exception e)
            {
                ViewData["ShowError"] = true;
                ViewData["Error"] = e.Message;
                return Page();
            }

            ViewData["ShowOk"] = true;
            return Page();
        }

        public async Task<IActionResult> OnPostUploadPhoto([FromForm]IFormFile formFile)
        {
            await using var memoryStream = new MemoryStream();

            await formFile.CopyToAsync(memoryStream);

            // Upload the file if less than 2 MB
            if (memoryStream.Length >= 2097152) return new JsonResult("no");
            
            var url = await _uploadService
                .UploadFileAsync(memoryStream, formFile.FileName, formFile.ContentType);
            
            return new JsonResult(url ?? "no");

        }
    }
}