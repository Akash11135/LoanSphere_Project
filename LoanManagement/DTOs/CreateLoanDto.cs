using System.ComponentModel.DataAnnotations;

namespace LoanManagement.DTOs
{
    public class CreateLoanDto
    {
        public string UserId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double Amount { get; set; }

        [Required]
        public string LoanType { get; set; } = string.Empty;

        [Required]
        public int TermInMonths { get; set; }

        public bool IsVerifiedByAdmin { get; set; }

        public bool IsRejected { get; set; }

        [MaxLength(100, ErrorMessage = "Maximum length of the purpose is 100.")]
        public string Purpose { get; set; } = string.Empty;

    }
}
