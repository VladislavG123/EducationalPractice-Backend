using GreenPoint.Dotnet.DataAccess.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenPoint.Dotnet.DataAccess.Models
{
    public class Spot : Entity
    {
        public double Langitude { get; set; }
        public double Latitude { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
    }
}
