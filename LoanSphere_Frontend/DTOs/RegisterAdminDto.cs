using System.ComponentModel.DataAnnotations;

namespace LoanSphere_Frontend.DTOs
{
    public class RegisterAdminDto
    {
        [Required]
        [MinLength(3)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter valid 10-digit phone")]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [RegularExpression(
            @"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$",
            ErrorMessage = "Min 8 chars, 1 uppercase, 1 number, 1 special char"
        )]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public string AdminSecretKey { get; set; } = string.Empty;
    }
}