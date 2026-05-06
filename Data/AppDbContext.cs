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
    }
}