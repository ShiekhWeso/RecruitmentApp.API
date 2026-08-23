namespace RecruitmentApp.API.Models
{
    public class DailyChallenge
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Field { get; set; } = string.Empty;
        public int QuestionCount { get; set; }
        public int EstimatedMinutes { get; set; }
        public int StudentsDoneToday { get; set; }
        public DateTime ChallengeDate { get; set; } = DateTime.UtcNow.Date;
        public bool IsActive { get; set; } = true;
    }
}