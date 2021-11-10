using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GreenPoint.Dotnet.DataAccess.Models;
using GreenPoint.Dotnet.DataAccess.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GreenPoint.Dotnet.WebAdmin.Pages
{
    public class StatusesPage : PageModel
    {
        private readonly StatusProvider _statusProvider;


        [BindProperty] public List<Status> Statuses { get; set; }

        public StatusesPage(StatusProvider statusProvider)
        {
            _statusProvider = statusProvider;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Statuses = await _statusProvider.GetAll();
            Statuses.Sort((status1, status2) => status2.MinValue - status1.MinValue);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromForm] string name, [FromForm] int min, [FromForm] int max)
        {
            if (max <= min)
            {
                ViewData["Error"] = "Минимальное значение должно быть больше максимального";
                return await OnGetAsync();
            }

            try
            {
                await _statusProvider.Add(new Status
                {
                    Name = name,
                    MaxValue = max,
                    MinValue = min
                });
            }
            catch (ArgumentException e)
            {
                ViewData["Error"] = "Введённый интервал пересекает один из старых";
            }

            return await OnGetAsync();
        }
    }
}