using System.ComponentModel.DataAnnotations;

namespace LoanSphere_Frontend.DTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression(
      @"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
      ErrorMessage = "Password must be at least 8 characters long and include 1 uppercase letter, 1 number, and 1 special character."
  )]
        public string Password { get; set; } = string.Empty;
    }
}
