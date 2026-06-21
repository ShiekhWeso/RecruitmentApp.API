namespace RecruitmentApp.API.Models
{
    public class Course
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Field { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool HasCertificate { get; set; } = true;
        public string InstructorName { get; set; } = string.Empty;
        public string InstructorTitle { get; set; } = string.Empty;
        public string InstructorBio { get; set; } = string.Empty;
        public double InstructorRating { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<CourseModule> Modules { get; set; } = new();
    }
}