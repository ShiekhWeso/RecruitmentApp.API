namespace RecruitmentApp.API.Models
{
    public class Enrollment
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }
        public Guid? CurrentLessonId { get; set; }
        public int ProgressPercent { get; set; } = 0;
        public bool IsCompleted { get; set; } = false;
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
        public User User { get; set; } = null!;
        public Course Course { get; set; } = null!;
    }
}