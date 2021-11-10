using GreenPoint.Dotnet.DataAccess.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenPoint.Dotnet.DataAccess.Models
{
    public class Like : Entity
    {
        public Guid UserId { get; set; }
        public Guid CommentId { get; set; }
    }
}
