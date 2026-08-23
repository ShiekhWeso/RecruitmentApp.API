namespace RecruitmentApp.API.Models
{
    public class StudyGroupMessage
    {
        public Guid Id { get; set; }
        public Guid StudyGroupId { get; set; }
        public Guid UserId { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public StudyGroup StudyGroup { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}