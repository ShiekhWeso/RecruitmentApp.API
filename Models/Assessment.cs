namespace RecruitmentApp.API.Models
{
    public class Assessment
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Field { get; set; } = string.Empty;
        public string Status { get; set; } = "in-progress";
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
        public User User { get; set; } = null!;
    }
}