namespace Contract_Monthly_System.Models
{
    public class CordinatorViewer
    {
        public int CordinatorId { get; set; }
        public int LecturerId { get; set; }
        public string Lecturer { get; set; } = "";
        public string Month { get; set; } = "";

    public double Total { get; set; }
        public ClaimState State { get; set; }


    }
}
