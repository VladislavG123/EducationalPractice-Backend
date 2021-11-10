using System;

namespace GreenPoint.Dotnet.Contracts.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string AvatarUrl { get; set; }
        public string Status { get; set; }
    }
}