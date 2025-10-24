namespace Contract_Monthly_System.Models
{
    public class ClaimDetail
    {
        public int DetailId { get; set; }
        public string Module { get; set; } = "";
        public double HoursWorked { get; set; }
        public double Rate { get; set; }
        public double LineTotal => Math.Round(HoursWorked * Rate, 2);
    }
}
