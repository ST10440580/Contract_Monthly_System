using Contract_Monthly_System.Models.Services;

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
        public List<ClaimListViewer> Claims { get; set; } = new();


    }
}
