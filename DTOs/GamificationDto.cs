namespace RecruitmentApp.API.DTOs
{
    public class UserBadgeDto
    {
        public Guid BadgeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public int XpReward { get; set; }
        public bool IsEarned { get; set; }
        public int Progress { get; set; }
        public int RequirementValue { get; set; }
        public DateTime? EarnedAt { get; set; }
    }

    public class MilestoneDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
    }

    public class GamificationProfileDto
    {
        public string Name { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public int TotalXp { get; set; }
        public string CurrentRank { get; set; } = string.Empty;
        public string TopPercentile { get; set; } = string.Empty;
        public int Level { get; set; }
        public List<UserBadgeDto> Badges { get; set; } = new();
        public List<MilestoneDto> Milestones { get; set; } = new();
    }
}