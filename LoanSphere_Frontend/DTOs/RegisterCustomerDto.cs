using System.ComponentModel.DataAnnotations;

namespace LoanSphere_Frontend.DTOs
{
    public class RegisterCustomerDto
    {
        [Required]
        [MinLength(3)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[6-9]\d{9}$")]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [RegularExpression(
            @"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$"
        )]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}