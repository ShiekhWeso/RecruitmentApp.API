namespace RecruitmentApp.API.DTOs
{
    public class DailyChallengeDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Field { get; set; } = string.Empty;
        public int QuestionCount { get; set; }
        public int EstimatedMinutes { get; set; }
        public int StudentsDoneToday { get; set; }
    }

    public class WeeklyPlanDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }

    public class WeeklyPlanProgressDto
    {
        public int ProgressPercent { get; set; }
        public List<WeeklyPlanDto> Items { get; set; } = new();
    }

    public class StudyGroupDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Field { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int MemberCount { get; set; }
        public int ActiveCount { get; set; }
        public bool IsMember { get; set; }
    }

    public class StudyGroupMessageDto
    {
        public Guid Id { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        public bool IsMe { get; set; }
    }

    public class SendMessageDto
    {
        public string Message { get; set; } = string.Empty;
    }

    public class LeaderboardEntryDto
    {
        public int Rank { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Points { get; set; }
        public bool IsMe { get; set; }
    }

    public class CareerProjectionDto
    {
        public string CurrentRank { get; set; } = string.Empty;
        public string ProjectedRank { get; set; } = string.Empty;
        public int CurrentXp { get; set; }
        public int JobMatchesToday { get; set; }
        public int JobMatchesAtProjection { get; set; }
        public List<string> ToGetThere { get; set; } = new();
    }

    public class HomeScreenDto
    {
        public string Name { get; set; } = string.Empty;
        public int SkillScore { get; set; }
        public string CurrentRank { get; set; } = string.Empty;
        public int XpThisWeek { get; set; }
        public DailyChallengeDto? TodaysChallenge { get; set; }
        public WeeklyPlanProgressDto WeeklyPlan { get; set; } = new();
        public StudyGroupDto? MyStudyGroup { get; set; }
    }
}