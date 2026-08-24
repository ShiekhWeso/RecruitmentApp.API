namespace RecruitmentApp.API.DTOs
{
    public class GapMapDto
    {
        public List<SkillNodeDto> Skills { get; set; } = new();
        public List<CompanyInterestedDto> CompaniesInterested { get; set; } = new();
    }

    public class SkillNodeDto
    {
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // Strong, Fair, Gap
    }

    public class CompanyInterestedDto
    {
        public string Name { get; set; } = string.Empty;
        public string Stage { get; set; } = string.Empty;
    }

    public class HiringModeDto
    {
        public bool IsActive { get; set; }
        public List<VisibilitySettingDto> VisibilitySettings { get; set; } = new();
    }

    public class VisibilitySettingDto
    {
        public string Label { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }

    public class UpdateHiringModeDto
    {
        public bool IsActive { get; set; }
    }

    public class RoadmapDto
    {
        public string DreamRole { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Salary { get; set; } = string.Empty;
        public int ExpertsTracking { get; set; }
        public bool TargetReached { get; set; }
        public List<RoadmapStepDto> Steps { get; set; } = new();
    }

    public class RoadmapStepDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Order { get; set; }
        public string TimeFrame { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public bool IsCurrent { get; set; }
    }

    public class MockInterviewDto
    {
        public Guid Id { get; set; }
        public int CurrentQuestion { get; set; }
        public int TotalQuestions { get; set; }
        public string CurrentQuestionText { get; set; } = string.Empty;
        public int VoiceConfidence { get; set; }
        public string EyeContact { get; set; } = string.Empty;
        public int FillerWordsCount { get; set; }
        public int TimeRemainingSeconds { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class SubmitInterviewAnswerDto
    {
        public string Answer { get; set; } = string.Empty;
    }

    public class NotificationDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class NoMatchesDto
    {
        public bool HasMatches { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Suggestions { get; set; } = new();
    }
}