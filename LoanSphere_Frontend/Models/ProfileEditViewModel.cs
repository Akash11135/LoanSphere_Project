using System.ComponentModel.DataAnnotations;

namespace LoanSphere_Frontend.Models
{
    public class ProfileEditViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        [Url]
        [Display(Name = "Profile picture URL")]
        public string? ProfilePictureUrl { get; set; }

        [Display(Name = "PAN number")]
        public string? PanCardNumber { get; set; }

        [Display(Name = "Aadhaar number")]
        public string? AadhaarNumber { get; set; }

        public int CibilScore { get; set; }
        public bool IsProfileComplete { get; set; }
    }
}
