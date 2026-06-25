using Microsoft.EntityFrameworkCore;
using RecruitmentApp.API.Data;
using RecruitmentApp.API.DTOs;
using RecruitmentApp.API.Models;

namespace RecruitmentApp.API.Services
{
    public class JobService : IJobService
    {
        private readonly AppDbContext _context;

        public JobService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<JobMatchListDto>> GetJobMatches(Guid userId, string? field = null, string? location = null, string? jobType = null)
        {
            var analysis = await _context.CvAnalyses.Where(a => a.UserId == userId).OrderByDescending(a => a.AnalyzedAt).FirstOrDefaultAsync();
            var query = _context.Jobs.Where(j => j.IsActive);

            if (!string.IsNullOrEmpty(field)) query = query.Where(j => j.Field == field);
            if (!string.IsNullOrEmpty(location)) query = query.Where(j => j.Location == location);
            if (!string.IsNullOrEmpty(jobType)) query = query.Where(j => j.JobType == jobType);

            var jobs = await query.ToListAsync();

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

        public async Task<JobDetailDto> GetJobDetail(Guid jobId, Guid userId)
        {
            var job = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == jobId);
            if (job == null) throw new Exception("Job not found");

            var analysis = await _context.CvAnalyses
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.AnalyzedAt)
                .FirstOrDefaultAsync();

            var userScore = analysis?.Score ?? 0;
            var matchPercentage = userScore >= job.MinScore
                ? Math.Min(100, userScore + new Random().Next(-10, 10))
                : Math.Max(30, userScore - new Random().Next(10, 20));

            var requirements = string.IsNullOrEmpty(job.Requirements)
                ? new List<string>()
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(job.Requirements) ?? new();

            var requiredSkills = string.IsNullOrEmpty(job.RequiredSkills)
                ? new List<JobSkillDto>()
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(job.RequiredSkills)!
                    .Select(skill => new JobSkillDto
                    {
                        SkillName = skill,
                        NeededScore = 70,
                        UserScore = userScore,
                        IsMet = userScore >= 70
                    }).ToList();

            return new JobDetailDto
            {
                Id = job.Id,
                Title = job.Title,
                Company = job.Company,
                Location = job.Location,
                JobType = job.JobType,
                MatchPercentage = matchPercentage,
                Field = job.Field,
                Description = job.Description,
                Requirements = requirements,
                RequiredSkills = requiredSkills
            };
        }

        public async Task<JobApplicationDto> ApplyToJob(Guid userId, ApplyJobDto dto)
        {
            var job = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == dto.JobId);
            if (job == null) throw new Exception("Job not found");

            var existing = await _context.JobApplications
                .FirstOrDefaultAsync(a => a.UserId == userId && a.JobId == dto.JobId);
            if (existing != null) throw new Exception("Already applied to this job");

            var analysis = await _context.CvAnalyses
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.AnalyzedAt)
                .FirstOrDefaultAsync();

            var userScore = analysis?.Score ?? 0;
            var matchPercentage = userScore >= job.MinScore
                ? Math.Min(100, userScore + new Random().Next(-10, 10))
                : Math.Max(30, userScore - new Random().Next(10, 20));

            var application = new JobApplication
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                JobId = dto.JobId,
                Status = "Pending",
                MatchPercentage = matchPercentage,
                AppliedAt = DateTime.UtcNow
            };

            _context.JobApplications.Add(application);
            await _context.SaveChangesAsync();

            return new JobApplicationDto
            {
                Id = application.Id,
                JobId = job.Id,
                Title = job.Title,
                Company = job.Company,
                Location = job.Location,
                JobType = job.JobType,
                Status = application.Status,
                MatchPercentage = matchPercentage,
                AppliedAt = application.AppliedAt
            };
        }

        public async Task<List<JobApplicationDto>> GetApplications(Guid userId, string? status = null)
        {
            var query = _context.JobApplications
                .Include(a => a.Job)
                .Where(a => a.UserId == userId);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(a => a.Status == status);

            var applications = await query.OrderByDescending(a => a.AppliedAt).ToListAsync();

            return applications.Select(a => new JobApplicationDto
            {
                Id = a.Id,
                JobId = a.Job.Id,
                Title = a.Job.Title,
                Company = a.Job.Company,
                Location = a.Job.Location,
                JobType = a.Job.JobType,
                Status = a.Status,
                MatchPercentage = a.MatchPercentage,
                AppliedAt = a.AppliedAt
            }).ToList();
        }

        public async Task<bool> WithdrawApplication(Guid userId, Guid applicationId)
        {
            var application = await _context.JobApplications
                .FirstOrDefaultAsync(a => a.Id == applicationId && a.UserId == userId);
            if (application == null) throw new Exception("Application not found");

            _context.JobApplications.Remove(application);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}