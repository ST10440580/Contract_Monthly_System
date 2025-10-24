using System.ComponentModel.DataAnnotations;

namespace Contract_Monthly_System.Models
{
    public class Lecturer
    {
      
        public int LectuerId { get; set; }
        public string LecturerName { get; set; } = "";
        public string Email { get; set; } = "";
        public double HourlyRate { get; set; }
        public List<ClaimListViewer> Claim { get; set; } = new List<ClaimListViewer>();


    }
}
