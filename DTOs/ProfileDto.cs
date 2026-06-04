namespace RecruitmentApp.API.DTOs
{
    public class SkillBreakdownDto
    {
        public string SkillName { get; set; } = string.Empty;
        public int Score { get; set; }
    }

    public class TestHistoryDto
    {
        public string Field { get; set; } = string.Empty;
        public int Score { get; set; }
        public DateTime CompletedAt { get; set; }
    }

    public class UserProfileDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Field { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string ExperienceLevel { get; set; } = string.Empty;
        public int SkillScore { get; set; }
        public bool IsVerified { get; set; }
        public string Role { get; set; } = string.Empty;
        public List<SkillBreakdownDto> SkillBreakdown { get; set; } = new();
        public List<TestHistoryDto> TestHistory { get; set; } = new();
    }
}