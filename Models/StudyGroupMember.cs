namespace RecruitmentApp.API.Models
{
    public class StudyGroupMember
    {
        public Guid Id { get; set; }
        public Guid StudyGroupId { get; set; }
        public Guid UserId { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        public StudyGroup StudyGroup { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}