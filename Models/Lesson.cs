namespace RecruitmentApp.API.Models
{
    public class Lesson
    {
        public Guid Id { get; set; }
        public Guid ModuleId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = "Video";
        public string Duration { get; set; } = string.Empty;
        public int Order { get; set; }
        public CourseModule Module { get; set; } = null!;
    }
}