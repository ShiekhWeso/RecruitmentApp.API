namespace RecruitmentApp.API.DTOs
{
    public class AssessmentMainDto
    {
        public int CompletedCount { get; set; }
        public int AvgScore { get; set; }
        public int Badges { get; set; }
        public string Rank { get; set; } = string.Empty;
        public List<RecommendedAssessmentDto> Recommended { get; set; } = new();
        public List<AvailableAssessmentDto> Available { get; set; } = new();
    }

    public class RecommendedAssessmentDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public int XpReward { get; set; }
    }

    public class AvailableAssessmentDto
    {
        public string Field { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public int XpReward { get; set; }
    }

    public class AssessmentHistoryDto
    {
        public Guid Id { get; set; }
        public string Field { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public int Score { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CompletedAt { get; set; }
    }
}