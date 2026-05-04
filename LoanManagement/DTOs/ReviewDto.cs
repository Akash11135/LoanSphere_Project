namespace LoanManagement.DTOs
{
    public class ReviewDto
    {
        public int LoanId { get; set; }
     
        public string DocsVerified
        { get; set; } = string.Empty;

        public bool IsVerifiedByAdmin { get; set; }

        public bool IsRejected { get; set; }

        public DateTime UpdatedAt { get; set; }

    }
}
