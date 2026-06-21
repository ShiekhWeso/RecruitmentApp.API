namespace RecruitmentApp.API.Models
{
    public class CourseModule
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Order { get; set; }
        public Course Course { get; set; } = null!;
        public List<Lesson> Lessons { get; set; } = new();
    }
}