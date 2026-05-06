namespace RecruitmentApp.API.Models
{
    public class CvAnalysis
    {
        public Guid Id { get; set; }
        public Guid CvId { get; set; }
        public Guid UserId { get; set; }
        public string Skills { get; set; } = string.Empty;
        public string Gaps { get; set; } = string.Empty;
        public int Score { get; set; }
        public string ExperienceLevel { get; set; } = string.Empty;
        public string Field { get; set; } = string.Empty;
        public DateTime AnalyzedAt { get; set; } = DateTime.UtcNow;
    }
}