using System.ComponentModel.DataAnnotations;

namespace LoanManagement.DTOs
{
    public class UpdateLoanDecisionDto
    {
        [Required]
        public string ReviewerRole { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? Reason { get; set; }
    }
}
