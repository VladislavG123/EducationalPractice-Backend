using System;
using System.Collections.Generic;

namespace GreenPoint.Dotnet.Contracts.ViewModels
{
    public class SpotViewModel
    {
        public Guid Id { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public List<string> Images { get; set; }
    }
}