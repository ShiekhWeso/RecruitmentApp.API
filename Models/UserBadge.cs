namespace RecruitmentApp.API.Models
{
    public class UserBadge
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BadgeId { get; set; }
        public int Progress { get; set; } = 0;
        public bool IsEarned { get; set; } = false;
        public DateTime? EarnedAt { get; set; }
        public User User { get; set; } = null!;
        public Badge Badge { get; set; } = null!;
    }
}