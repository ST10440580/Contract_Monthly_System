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


    }
}
