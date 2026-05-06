using Microsoft.EntityFrameworkCore;
using RecruitmentApp.API.Data;
using RecruitmentApp.API.DTOs;

namespace RecruitmentApp.API.Services
{
    public class JobService : IJobService
    {
        private readonly AppDbContext _context;

        public JobService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<JobMatchListDto>> GetJobMatches(Guid userId)
        {
            var analysis = await _context.CvAnalyses.Where(a => a.UserId == userId).OrderByDescending(a => a.AnalyzedAt).FirstOrDefaultAsync();
            var jobs = await _context.Jobs.Where(j => j.IsActive).ToListAsync();

            if (!jobs.Any())
            {
                return new List<JobMatchListDto>
                {
                    new JobMatchListDto { Id = Guid.NewGuid(), Title = "Senior Frontend Developer", Company = "FintechCorp", Location = "Cairo", JobType = "Hybrid", MatchPercentage = 92, Field = "Software Dev" },
                    new JobMatchListDto { Id = Guid.NewGuid(), Title = "React Engineer", Company = "GlobalTech", Location = "Remote", JobType = "Full-time", MatchPercentage = 82, Field = "Software Dev" },
                    new JobMatchListDto { Id = Guid.NewGuid(), Title = "UI/UX Developer", Company = "DesignStudio", Location = "Cairo", JobType = "On-site", MatchPercentage = 74, Field = "Design" },
                    new JobMatchListDto { Id = Guid.NewGuid(), Title = "Web Developer", Company = "AgencyX", Location = "Remote", JobType = "Contract", MatchPercentage = 67, Field = "Software Dev" },
                    new JobMatchListDto { Id = Guid.NewGuid(), Title = "Software Engineer", Company = "HealthPlus", Location = "Cairo", JobType = "Full-time", MatchPercentage = 60, Field = "Software Dev" }
                };
            }

            var userScore = analysis?.Score ?? 0;
            return jobs.Select(j => new JobMatchListDto
            {
                Id = j.Id,
                Title = j.Title,
                Company = j.Company,
                Location = j.Location,
                JobType = j.JobType,
                MatchPercentage = userScore >= j.MinScore ? Math.Min(100, userScore + new Random().Next(-10, 10)) : Math.Max(30, userScore - new Random().Next(10, 20)),
                Field = j.Field
            }).OrderByDescending(j => j.MatchPercentage).ToList();
        }
    }
}