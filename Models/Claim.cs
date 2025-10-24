using Contract_Monthly_System.Models.Services;

namespace Contract_Monthly_System.Models
{
    public class Claim
    {
        public int LecturerID { get; set; }

        public int ClaimId { get; set; }
        public string LecturerName { get; set; }
        public string Module { get; set; }
        public string Month { get; set; }
        public double HoursWorked { get; set; }
        public double HourlyRate { get; set; }
        public string Notes { get; set; }
        public ClaimState State { get; set; }

        public string SupportingDocumentPath { get; set; }
        public decimal TotalAmount { get; set; } 
        public double Total => HoursWorked * HourlyRate;
    }
}