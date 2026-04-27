namespace LoanManagement.DTOs
{
    public class UpdateLoanDto
    {
        public int UserId {  get; set; }
        public int LoanId { get; set; }
        public string Status {  get; set; }
        public string DocsVerified { get; set; }
        public string Purpose { get; set; }
    }
}
