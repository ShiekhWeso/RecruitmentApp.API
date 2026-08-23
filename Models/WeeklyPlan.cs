namespace RecruitmentApp.API.Models
{
    public class WeeklyPlan
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
        public int WeekNumber { get; set; }
        public int Year { get; set; }
        public User User { get; set; } = null!;
    }
}