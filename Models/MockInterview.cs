namespace RecruitmentApp.API.Models
{
    public class MockInterview
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Status { get; set; } = "in-progress";
        public int CurrentQuestion { get; set; } = 1;
        public int TotalQuestions { get; set; } = 8;
        public int VoiceConfidence { get; set; } = 0;
        public string EyeContact { get; set; } = "Good";
        public int FillerWordsCount { get; set; } = 0;
        public int TimeRemainingSeconds { get; set; } = 262;
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
        public User User { get; set; } = null!;
    }
}