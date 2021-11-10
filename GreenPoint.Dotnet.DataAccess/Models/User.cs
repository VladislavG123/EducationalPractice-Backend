using GreenPoint.Dotnet.DataAccess.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenPoint.Dotnet.DataAccess.Models
{
    public class User : Entity
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Guid AvatarId { get; set; }
    }
}
