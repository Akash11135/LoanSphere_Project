namespace LoanManagement.Models
{
    public class ApiResponse<T>
    {
        public int Status { get; set; }
        public String Mess { get; set; } = string.Empty;
        public T Data { get; set; }

        public ApiResponse(int Status , string Mess ,T Data)
        {
            this.Status = Status;
            this.Mess = Mess;
            this.Data = Data;
        }
    }
}
