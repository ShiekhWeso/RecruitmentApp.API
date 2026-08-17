namespace RecruitmentApp.API.Models
{
    public class Milestone
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
        public DateTime? CompletedAt { get; set; }
        public User User { get; set; } = null!;
    }
}