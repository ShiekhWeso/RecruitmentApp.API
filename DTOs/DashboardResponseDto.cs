namespace RecruitmentApp.API.DTOs
{
    public class DashboardResponseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Field { get; set; } = string.Empty;
        public int SkillScore { get; set; }
        public bool IsVerified { get; set; }
        public List<JobMatchDto> JobMatches { get; set; } = new();
    }

    public class JobMatchDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int MatchPercentage { get; set; }
    }
}