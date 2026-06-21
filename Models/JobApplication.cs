namespace RecruitmentApp.API.Models
{
    public class JobApplication
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid JobId { get; set; }
        public string Status { get; set; } = "Pending";
        public int MatchPercentage { get; set; }
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
        public User User { get; set; } = null!;
        public Job Job { get; set; } = null!;
    }
}