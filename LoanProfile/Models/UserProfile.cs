using System.ComponentModel.DataAnnotations;

namespace LoanProfile.Models
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty; // From Auth Service

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public string? ProfilePictureUrl { get; set; }

        public string? PanCardNumber { get; set; }
        public string? AadhaarNumber { get; set; }

        public int CibilScore { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}