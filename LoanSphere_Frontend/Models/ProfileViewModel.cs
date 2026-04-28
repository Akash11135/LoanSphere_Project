namespace LoanSphere_Frontend.Models
{
    public class ProfileViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
        public string? PanCardNumber { get; set; }
        public string? AadhaarNumber { get; set; }
        public int CibilScore { get; set; }
        public bool IsProfileComplete { get; set; }
    }
}
