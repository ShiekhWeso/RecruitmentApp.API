using Microsoft.EntityFrameworkCore;
using RecruitmentApp.API.Data;
using RecruitmentApp.API.DTOs;
using RecruitmentApp.API.Models;

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

        public async Task<List<CourseListDto>> GetCourses(Guid userId, string? field = null)
        {
            var query = _context.Courses.Where(c => c.IsActive);
            if (!string.IsNullOrEmpty(field))
                query = query.Where(c => c.Field == field);

            var courses = await query.ToListAsync();
            var enrollments = await _context.Enrollments
                .Where(e => e.UserId == userId).ToListAsync();

            return courses.Select(c => new CourseListDto
            {
                Id = c.Id,
                Title = c.Title,
                Field = c.Field,
                Level = c.Level,
                Duration = c.Duration,
                Price = c.Price,
                InstructorName = c.InstructorName,
                InstructorRating = c.InstructorRating,
                IsEnrolled = enrollments.Any(e => e.CourseId == c.Id)
            }).ToList();
        }

        public async Task<CourseDetailDto> GetCourseDetail(Guid courseId, Guid userId)
        {
            var course = await _context.Courses
                .Include(c => c.Modules)
                .ThenInclude(m => m.Lessons)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null) throw new Exception("Course not found");

            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.CourseId == courseId && e.UserId == userId);

            return new CourseDetailDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Field = course.Field,
                Level = course.Level,
                Duration = course.Duration,
                Price = course.Price,
                HasCertificate = course.HasCertificate,
                InstructorName = course.InstructorName,
                InstructorTitle = course.InstructorTitle,
                InstructorBio = course.InstructorBio,
                InstructorRating = course.InstructorRating,
                IsEnrolled = enrollment != null,
                ProgressPercent = enrollment?.ProgressPercent ?? 0,
                Modules = course.Modules.OrderBy(m => m.Order).Select(m => new CourseModuleDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Order = m.Order,
                    Lessons = m.Lessons.OrderBy(l => l.Order).Select(l => new LessonDto
                    {
                        Id = l.Id,
                        Title = l.Title,
                        Type = l.Type,
                        Duration = l.Duration,
                        Order = l.Order
                    }).ToList()
                }).ToList()
            };
        }

        public async Task<EnrollmentDto> EnrollInCourse(Guid userId, Guid courseId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
            if (course == null) throw new Exception("Course not found");

            var existing = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId);
            if (existing != null) throw new Exception("Already enrolled in this course");

            var enrollment = new Enrollment
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CourseId = courseId,
                ProgressPercent = 0,
                IsCompleted = false,
                EnrolledAt = DateTime.UtcNow
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return new EnrollmentDto
            {
                EnrollmentId = enrollment.Id,
                CourseId = courseId,
                CourseTitle = course.Title,
                ProgressPercent = 0,
                IsCompleted = false,
                EnrolledAt = enrollment.EnrolledAt
            };
        }

        public async Task<List<EnrollmentDto>> GetMyEnrollments(Guid userId)
        {
            var enrollments = await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.EnrolledAt)
                .ToListAsync();

            return enrollments.Select(e => new EnrollmentDto
            {
                EnrollmentId = e.Id,
                CourseId = e.CourseId,
                CourseTitle = e.Course.Title,
                ProgressPercent = e.ProgressPercent,
                IsCompleted = e.IsCompleted,
                EnrolledAt = e.EnrolledAt
            }).ToList();
        }

        public async Task<EnrollmentDto> UpdateProgress(Guid userId, Guid enrollmentId, UpdateProgressDto dto)
        {
            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.Id == enrollmentId && e.UserId == userId);

            if (enrollment == null) throw new Exception("Enrollment not found");

            enrollment.CurrentLessonId = dto.LessonId;
            enrollment.ProgressPercent = dto.ProgressPercent;

            if (dto.ProgressPercent >= 100)
            {
                enrollment.IsCompleted = true;
                enrollment.CompletedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return new EnrollmentDto
            {
                EnrollmentId = enrollment.Id,
                CourseId = enrollment.CourseId,
                CourseTitle = enrollment.Course.Title,
                ProgressPercent = enrollment.ProgressPercent,
                IsCompleted = enrollment.IsCompleted,
                EnrolledAt = enrollment.EnrolledAt
            };
        }
    }   
}