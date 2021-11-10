using System;

namespace GreenPoint.Dotnet.DataAccess.Models
{
    public class UserCode
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public Guid UserId { get; set; }
    }
}