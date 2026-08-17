using Microsoft.EntityFrameworkCore;
using RecruitmentApp.API.Models;

namespace RecruitmentApp.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; } 
        public DbSet<CvUpload> CvUploads { get; set; }
        public DbSet<CvAnalysis> CvAnalyses { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseModule> CourseModules { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<UserBadge> UserBadges { get; set; }
        public DbSet<Milestone> Milestones { get; set; }
    }
}