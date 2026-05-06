namespace RecruitmentApp.API.Models
{
    public class Job
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string JobType { get; set; } = string.Empty;
        public string Field { get; set; } = string.Empty;
        public int MinScore { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime PostedAt { get; set; } = DateTime.UtcNow;
    }
}