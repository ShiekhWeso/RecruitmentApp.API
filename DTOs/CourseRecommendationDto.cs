namespace RecruitmentApp.API.DTOs
{
    public class CourseDto
    {   
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Platform { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string Gap { get; set; } = string.Empty;
        public int Points { get; set; }
        public bool IsPriority { get; set; }
    }

    public class LearningPathDto
    {
        public List<string> Gaps { get; set; } = new();
        public List<CourseDto> Courses { get; set; } = new();

    }
}