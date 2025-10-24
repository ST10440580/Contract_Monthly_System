namespace Contract_Monthly_System.Models
{
    public class Document_Submission
    {
        public int DocumentId { get; set; }
        public string FileName { get; set; } = "";
        public string FilePath { get; set; } = "";
        public DateTime UploadedOn { get; set; } = DateTime.UtcNow;
    }
}
