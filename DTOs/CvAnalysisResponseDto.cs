namespace RecruitmentApp.API.DTOs
{
    public class CvAnalysisResponseDto
    {
        public Guid CvId { get; set; }
        public List<string> Skills { get; set; } = new();
        public List<string> Gaps { get; set; } = new();
        public int Score { get; set; }
        public string ExperienceLevel { get; set; } = string.Empty;
        public string Field { get; set; } = string.Empty;
    }
}