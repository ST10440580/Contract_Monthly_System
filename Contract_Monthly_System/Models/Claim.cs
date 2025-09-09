namespace Contract_Monthly_System.Models
{
    public class Claim
    {
        public int ClaimId { get; set; }
        public int LectureId { get; set; }
        
        public DateOnly Month { get; set; }
        public DateTime SubmissionDate { get; set; }
        public double TotalAmount { get; set; }
        public ClaimState State { get; set; } = ClaimState.Draft;
        public List<ClaimDetail> Details { get; set; } = new();
    }
}
