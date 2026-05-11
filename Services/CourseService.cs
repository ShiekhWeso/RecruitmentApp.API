using Microsoft.EntityFrameworkCore;
using RecruitmentApp.API.Data;
using RecruitmentApp.API.DTOs;

namespace RecruitmentApp.API.Services
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _context;
        
        public CourseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LearningPathDto> GetRecommendations(Guid userId)
        {
            var analysis = await _context.CvAnalyses.Where(a => a.UserId == userId).OrderByDescending(a => a.AnalyzedAt).FirstOrDefaultAsync();
            var assessment = await _context.Assessments.Where(a => a.UserId == userId && a.Status == "completed").OrderByDescending(a => a.CompletedAt).FirstOrDefaultAsync();

            var gaps = new List<string>();
            if (analysis != null)
            {
                gaps = System.Text.Json.JsonSerializer.Deserialize<List<string>>(analysis.Gaps) ?? new();
            }

            if (assessment != null)
            {
                gaps.Add("Hooks & useEffect");
                gaps.Add("State Management");
            }

            gaps = gaps.Distinct().ToList();

            var courses = new List<CourseDto>
            {
                new CourseDto
                {
                    Id = Guid.NewGuid(),
                    Title = "Master React Hooks",
                    Platform = "Udemy",
                    Duration = "6.5 hrs",
                    Level = "Intermediate",
                    Gap = "Hooks & useEffect",
                    Points = 12,
                    IsPriority = true
                },
                new CourseDto
                {
                    Id = Guid.NewGuid(),
                    Title = "Redux & State Management",
                    Platform = "Scrimba",
                    Duration = "4 hrs",
                    Level = "Intermediate",
                    Gap = "State Management",
                    Points = 8,
                    IsPriority = false
                },
                new CourseDto
                {
                    Id = Guid.NewGuid(),
                    Title = "Docker for Developers",
                    Platform = "Udemy",
                    Duration = "5 hrs",
                    Level = "Beginner",
                    Gap = "Docker",
                    Points = 5,
                    IsPriority = false
                },
                new CourseDto
                {
                    Id = Guid.NewGuid(),
                    Title = "System Design Fundamentals",
                    Platform = "Coursera",
                    Duration = "8 hrs",
                    Level = "Advanced",
                    Gap = "System Design",
                    Points = 5,
                    IsPriority = false
                }
            };

            return new LearningPathDto
            {
                Gaps = gaps,
                Courses = courses
            };
        }
    }   
}