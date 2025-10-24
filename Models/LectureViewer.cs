using Contract_Monthly_System.Models.Services;
using Contract_Monthly_System.Models;

namespace Contract_Monthly_System.Models
{
    public class LectureViewer

    {
        public int LecturerID { get; set; }

        public int ClaimID { get; set; }
        public string LecturerName { get; set; }
        public string Month { get; set; }
        public string Module { get; set; }
        public double HoursWorked { get; set; }
        public double HourlyRate { get; set; }
        public string Notes { get; set; }
        public ClaimState ClaimState { get; set; }
        public double Total { get; set; } // ✅ Writable

        public string SupportingDocumentPath { get; set; }

        public List<LectureViewer> Claims { get; set; } = new();
    }


}

public class ClaimListViewer
    {
        public string Month { get; set; } = "";
        public string Module { get; set; } = "";
        public double Hours { get; set; }
        public double Total { get; set; }
        public double TotalAmount { get; set; }
        public string Details { get; set; }
        public ClaimState State { get; set; }
      public int HoursWorked { get; set; }
    }
