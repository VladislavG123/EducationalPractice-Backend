using System;

namespace GreenPoint.Dotnet.Contracts.Parameters
{
    public class PatchUserParameter
    {
        public string Username { get; set; }
        public Guid AvatarId { get; set; }
        public string Password { get; set; }
    }
}