namespace RecruitmentApp.API.Models
{
    public class StudyGroup
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Field { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int MemberCount { get; set; } = 0;
        public int ActiveCount { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<StudyGroupMember> Members { get; set; } = new();
        public List<StudyGroupMessage> Messages { get; set; } = new();
    }
}