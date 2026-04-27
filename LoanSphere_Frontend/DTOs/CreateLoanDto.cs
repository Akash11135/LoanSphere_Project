using System.ComponentModel.DataAnnotations;

namespace LoanSphere_Frontend.DTOs
{
    public class CreateLoanDto
    {
        [Required]
        [Range(10000, 25000000, ErrorMessage = "Amount must be between ₹10,000 and ₹2,50,00,000")]
        public decimal Amount { get; set; }

        [Required]
        public int TermInMonths { get; set; }

        [Required]
        public string LoanType { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string Purpose { get; set; } = string.Empty;

        // File uploads
        [Required(ErrorMessage = "Government ID is required")]
        public IFormFile? GovernmentId { get; set; }

        [Required(ErrorMessage = "Proof of income is required")]
        public IFormFile? ProofOfIncome { get; set; }

        // Terms agreement
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept terms")]
        public bool AgreeTerms { get; set; }
    }
}
