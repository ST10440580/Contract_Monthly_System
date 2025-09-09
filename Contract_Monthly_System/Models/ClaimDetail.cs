namespace Contract_Monthly_System.Models
{
    public class ClaimDetail
    {

        public int DetailId { get; set; }
        public int ClaimId { get; set; }
        public string Module { get; set; } = "";
        public double HoursWorked { get; set; }
        public double Rate { get; set; }

    }
}
