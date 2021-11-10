using System.ComponentModel.DataAnnotations;

namespace GreenPoint.Dotnet.Contracts.Parameters
{
    public class SignInParameter
    {
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}