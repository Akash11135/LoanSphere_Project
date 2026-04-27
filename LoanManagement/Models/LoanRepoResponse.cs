namespace LoanManagement.Models
{
    public class LoanRepoResponse
    {
        public Loan loan { get; set; }
        public string Mess {  get; set; }

        public LoanRepoResponse(Loan loan ,  string mess)
        {
            this.loan = loan;
            this.Mess = mess;
        }
    }
}
