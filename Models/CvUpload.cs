namespace RecruitmentApp.API.Models
{
    public class CvUpload
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string Status { get; set; } = "uploaded";
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public User User { get; set; } = null!;
    }
}