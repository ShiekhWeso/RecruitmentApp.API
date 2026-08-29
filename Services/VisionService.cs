using Microsoft.EntityFrameworkCore;
using RecruitmentApp.API.Data;
using RecruitmentApp.API.DTOs;
using RecruitmentApp.API.Models;

namespace RecruitmentApp.API.Services
{
    public class VisionService : IVisionService
    {
        private readonly AppDbContext _context;

        public VisionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GapMapDto> GetGapMap(Guid userId)
        {
            var analysis = await _context.CvAnalyses
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.AnalyzedAt)
                .FirstOrDefaultAsync();

            var skills = new List<SkillNodeDto>();
            if (analysis != null)
            {
                var strongSkills = System.Text.Json.JsonSerializer.Deserialize<List<string>>(analysis.Skills) ?? new();
                var gapSkills = System.Text.Json.JsonSerializer.Deserialize<List<string>>(analysis.Gaps) ?? new();

                skills.AddRange(strongSkills.Select(s => new SkillNodeDto { Name = s, Status = "Strong" }));
                skills.AddRange(gapSkills.Select(s => new SkillNodeDto { Name = s, Status = "Gap" }));
            }
            else
            {
                skills = new List<SkillNodeDto>
                {
                    new() { Name = "React", Status = "Strong" },
                    new() { Name = "Hooks", Status = "Strong" },
                    new() { Name = "State", Status = "Fair" },
                    new() { Name = "Props", Status = "Strong" },
                    new() { Name = "Context", Status = "Fair" },
                    new() { Name = "Router", Status = "Strong" },
                    new() { Name = "Redux", Status = "Gap" },
                    new() { Name = "Zustand", Status = "Gap" },
                    new() { Name = "Next.js", Status = "Fair" },
                    new() { Name = "HTML", Status = "Strong" },
                    new() { Name = "CSS", Status = "Strong" },
                    new() { Name = "JS", Status = "Strong" },
                    new() { Name = "TS", Status = "Fair" },
                    new() { Name = "Jest", Status = "Gap" },
                    new() { Name = "API", Status = "Strong" },
                    new() { Name = "Git", Status = "Strong" },
                    new() { Name = "Webpack", Status = "Gap" },
                    new() { Name = "Vite", Status = "Fair" },
                    new() { Name = "A11y", Status = "Gap" },
                    new() { Name = "CI/CD", Status = "Gap" }
                };
            }

            return new GapMapDto
            {
                Skills = skills,
                CompaniesInterested = new List<CompanyInterestedDto>
                {
                    new() { Name = "Standard Chartered", Stage = "Sent an interview request" },
                    new() { Name = "Emirates NBB", Stage = "Sent an interview request" }
                }
            };
        }

        public async Task<HiringModeDto> GetHiringMode(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new Exception("User not found");

            return new HiringModeDto
            {
                IsActive = user.ProfileVisible,
                VisibilitySettings = new List<VisibilitySettingDto>
                {
                    new() { Label = "Skill Score & Rank", Value = "Visible" },
                    new() { Label = "Work History", Value = "Anonymous" },
                    new() { Label = "Current Employer", Value = "Hidden" },
                    new() { Label = "Full Name", Value = user.IncognitoMode ? "Hidden" : "Visible" }
                }
            };
        }

        public async Task<HiringModeDto> UpdateHiringMode(Guid userId, UpdateHiringModeDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new Exception("User not found");

            user.ProfileVisible = dto.IsActive;
            await _context.SaveChangesAsync();

            return await GetHiringMode(userId);
        }

        public async Task<RoadmapDto> GetRoadmap(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new Exception("User not found");

            var steps = await _context.RoadmapSteps
                .Where(r => r.UserId == userId)
                .OrderBy(r => r.Order)
                .ToListAsync();

            if (!steps.Any())
            {
                var defaultSteps = new List<RoadmapStep>
                {
                    new() { Id = Guid.NewGuid(), UserId = userId, Title = "Verified Candidate ★", Description = "Core profile & identity verification complete.", Order = 1, TimeFrame = "NOW", IsCompleted = true, IsCurrent = false },
                    new() { Id = Guid.NewGuid(), UserId = userId, Title = "Senior Talent ★★", Description = "Demonstrate leadership in current role and mentorship metrics.", Order = 2, TimeFrame = "3 MONTHS", IsCompleted = false, IsCurrent = true },
                    new() { Id = Guid.NewGuid(), UserId = userId, Title = "React + System Design tests", Description = "Pass advanced technical assessments.", Order = 3, TimeFrame = "6 MONTHS", IsCompleted = false, IsCurrent = false },
                    new() { Id = Guid.NewGuid(), UserId = userId, Title = "Build portfolio project", Description = "Complete and publish a full-stack project.", Order = 4, TimeFrame = "9 MONTHS", IsCompleted = false, IsCurrent = false },
                    new() { Id = Guid.NewGuid(), UserId = userId, Title = "Candidate Elite ★★★", Description = "The highest Hirley verification tier for MENA experts.", Order = 5, TimeFrame = "12 MONTHS", IsCompleted = false, IsCurrent = false }
                };

                _context.RoadmapSteps.AddRange(defaultSteps);
                await _context.SaveChangesAsync();
                steps = defaultSteps;
            }

            var completedAssessments = await _context.Assessments
                .CountAsync(a => a.UserId == userId && a.Status == "completed");
            var totalXp = (completedAssessments * 100) + (user.HasCv ? 50 : 0);
            var targetReached = totalXp >= 5000;

            return new RoadmapDto
            {
                DreamRole = $"Senior {user.Specialization ?? "Developer"} @ Google",
                Location = user.Locatoin ?? "Cairo",
                Salary = "35,000 EGP / mo",
                ExpertsTracking = 4,
                TargetReached = targetReached,
                Steps = steps.Select(s => new RoadmapStepDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    Description = s.Description,
                    Order = s.Order,
                    TimeFrame = s.TimeFrame,
                    IsCompleted = s.IsCompleted,
                    IsCurrent = s.IsCurrent
                }).ToList()
            };
        }

        public async Task<MockInterviewDto> StartMockInterview(Guid userId)
        {
            var interview = new MockInterview
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Status = "in-progress",
                CurrentQuestion = 1,
                TotalQuestions = 8,
                VoiceConfidence = 74,
                EyeContact = "Good",
                FillerWordsCount = 8,
                TimeRemainingSeconds = 262,
                StartedAt = DateTime.UtcNow
            };

            _context.MockInterviews.Add(interview);
            await _context.SaveChangesAsync();

            return MapToInterviewDto(interview);
        }

        public async Task<MockInterviewDto> SubmitInterviewAnswer(Guid userId, Guid interviewId, SubmitInterviewAnswerDto dto)
        {
            var interview = await _context.MockInterviews
                .FirstOrDefaultAsync(i => i.Id == interviewId && i.UserId == userId);
            if (interview == null) throw new Exception("Interview not found");

            if (interview.CurrentQuestion < interview.TotalQuestions)
            {
                interview.CurrentQuestion++;
                interview.TimeRemainingSeconds = 262;
                var random = new Random();
                interview.VoiceConfidence = random.Next(65, 90);
                interview.FillerWordsCount = random.Next(3, 12);
            }
            else
            {
                interview.Status = "completed";
                interview.CompletedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return MapToInterviewDto(interview);
        }

        public async Task<List<NotificationDto>> GetNotifications(Guid userId, string? type = null)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId)
                .ToListAsync();

            if (!notifications.Any())
            {
                var mockNotifications = new List<Notification>
                {
                    new() { Id = Guid.NewGuid(), UserId = userId, Type = "ScoreUpdate", Title = "Score Update", Message = "Your React assessment score was updated from 74 to 82.", IsRead = false, CreatedAt = DateTime.UtcNow.AddHours(-2) },
                    new() { Id = Guid.NewGuid(), UserId = userId, Type = "Achievement", Title = "Achievement", Message = "You are now a Verified Candidate in the top 5% of MENA engineering talent.", IsRead = false, CreatedAt = DateTime.UtcNow.AddHours(-5) },
                    new() { Id = Guid.NewGuid(), UserId = userId, Type = "ProfileView", Title = "Profile View", Message = "Incorta viewed your profile for the Senior Frontend Architect position.", IsRead = true, CreatedAt = DateTime.UtcNow.AddDays(-1) },
                    new() { Id = Guid.NewGuid(), UserId = userId, Type = "MatchFound", Title = "Match Found", Message = "A 91% skill match was found for a new Remote Engineering Manager role.", IsRead = true, CreatedAt = DateTime.UtcNow.AddDays(-2) },
                    new() { Id = Guid.NewGuid(), UserId = userId, Type = "StudyGroup", Title = "Study Group", Message = "Your answer in 'System Design MENA' received 12 new upvotes.", IsRead = true, CreatedAt = DateTime.UtcNow.AddDays(-3) }
                };

                _context.Notifications.AddRange(mockNotifications);
                await _context.SaveChangesAsync();
                notifications = mockNotifications;
            }

            if (!string.IsNullOrEmpty(type))
                notifications = notifications.Where(n => n.Type == type).ToList();

            return notifications
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new NotificationDto
                {
                    Id = n.Id,
                    Type = n.Type,
                    Title = n.Title,
                    Message = n.Message,
                    IsRead = n.IsRead,
                    CreatedAt = n.CreatedAt
                }).ToList();
        }

        public async Task<bool> MarkNotificationRead(Guid userId, Guid notificationId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);
            if (notification == null) throw new Exception("Notification not found");

            notification.IsRead = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkAllNotificationsRead(Guid userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            foreach (var n in notifications) n.IsRead = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<NoMatchesDto> GetJobMatchStatus(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new Exception("User not found");

            var analysis = await _context.CvAnalyses
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.AnalyzedAt)
                .FirstOrDefaultAsync();

            var userScore = analysis?.Score ?? 0;
            var hasMatches = await _context.Jobs
                .AnyAsync(j => j.IsActive && j.MinScore <= userScore);

            return new NoMatchesDto
            {
                HasMatches = hasMatches,
                Message = hasMatches ? "You have job matches!" : "Your skill score is below the minimum required for current openings in your region.",
                Suggestions = hasMatches ? new() : new List<string>
                {
                    "Take a test",
                    "Fix a gap",
                    "Lower filters"
                }
            };
        }

        private MockInterviewDto MapToInterviewDto(MockInterview interview)
        {
            var questions = new[]
            {
                "Tell me about a project you're proud of.",
                "How do you handle tight deadlines?",
                "Describe a challenging technical problem you solved.",
                "How do you approach code reviews?",
                "What's your experience with system design?",
                "How do you stay updated with new technologies?",
                "Describe your ideal work environment.",
                "Where do you see yourself in 5 years?"
            };

            return new MockInterviewDto
            {
                Id = interview.Id,
                CurrentQuestion = interview.CurrentQuestion,
                TotalQuestions = interview.TotalQuestions,
                CurrentQuestionText = questions[interview.CurrentQuestion - 1],
                VoiceConfidence = interview.VoiceConfidence,
                EyeContact = interview.EyeContact,
                FillerWordsCount = interview.FillerWordsCount,
                TimeRemainingSeconds = interview.TimeRemainingSeconds,
                Status = interview.Status
            };
        }
    }
}