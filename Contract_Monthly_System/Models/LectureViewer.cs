namespace Contract_Monthly_System.Models
{
    public class LectureViewer
    {

        public int LecturerId { get; set; }

        public string LecturerName { get; set; } = "";
        public double HourlyRate { get; set; }
        public List<ClaimListViewer> Claims { get; set; } = new();


    }

    public class ClaimListViewer
    {
        public string Month { get; set; } = "";
        public string Module { get; set; } = "";
        public double Hours { get; set; }
        public double Total { get; set; }
        public ClaimState State { get; set; }



    }
}
