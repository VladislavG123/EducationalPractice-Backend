using System.ComponentModel.DataAnnotations;

namespace GreenPoint.Dotnet.Contracts.Parameters
{
    public class SignUpParameter
    {
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}