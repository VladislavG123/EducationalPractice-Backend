using GreenPoint.Dotnet.DataAccess.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenPoint.Dotnet.DataAccess.Models
{
    public class Comment : Entity
    {
        public Guid UserId { get; set; }
        public Guid SpotId { get; set; }
        public string Text { get; set; }

    }
}
