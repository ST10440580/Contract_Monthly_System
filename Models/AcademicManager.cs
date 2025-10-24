using Contract_Monthly_System.Models.Services;

namespace Contract_Monthly_System.Models
{
    public class AcademicManager
    {


        public int ManagerId { get; set; }
        public string AcademicManagerName { get; set; } = "";

        public string ModuleName { get; set; } = "";
        public string Month { get; set; } = "";

        public double Total { get; set; }
        public ClaimState State { get; set; }
        public List<ClaimListViewer> Claims { get; set; } = new();
    }
}
