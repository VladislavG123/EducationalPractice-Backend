using GreenPoint.Dotnet.DataAccess.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenPoint.Dotnet.DataAccess.Models
{
    public class Admin : Entity
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
