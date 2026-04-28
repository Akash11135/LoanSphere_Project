using System.ComponentModel.DataAnnotations;

namespace LoanSphere_Frontend.Models
{
    public class LoanApplicationViewModel
    {
        [Required]
        [Range(1, double.MaxValue)]
        public double Amount { get; set; }

        [Required]
        public string LoanType { get; set; } = string.Empty;

        [Required]
        [Range(1, 600)]
        [Display(Name = "Term (months)")]
        public int TermInMonths { get; set; }

        [Required]
        [MaxLength(100)]
        public string Purpose { get; set; } = string.Empty;
    }
}
