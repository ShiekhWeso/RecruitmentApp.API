namespace RecruitmentApp.API.DTOs
{
    public class CourseListDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Field { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string InstructorName { get; set; } = string.Empty;
        public double InstructorRating { get; set; }
        public bool IsEnrolled { get; set; }
    }

    public class CourseDetailDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Field { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool HasCertificate { get; set; }
        public string InstructorName { get; set; } = string.Empty;
        public string InstructorTitle { get; set; } = string.Empty;
        public string InstructorBio { get; set; } = string.Empty;
        public double InstructorRating { get; set; }
        public bool IsEnrolled { get; set; }
        public int ProgressPercent { get; set; }
        public List<CourseModuleDto> Modules { get; set; } = new();
    }

    public class CourseModuleDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Order { get; set; }
        public List<LessonDto> Lessons { get; set; } = new();
    }

    public class LessonDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public int Order { get; set; }
    }

    public class EnrollmentDto
    {
        public Guid EnrollmentId { get; set; }
        public Guid CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;
        public int ProgressPercent { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime EnrolledAt { get; set; }
    }

    public class UpdateProgressDto
    {
        public Guid LessonId { get; set; }
        public int ProgressPercent { get; set; }
    }
}