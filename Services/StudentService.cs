using Microsoft.EntityFrameworkCore;
using RecruitmentApp.API.Data;
using RecruitmentApp.API.DTOs;
using RecruitmentApp.API.Models;

namespace RecruitmentApp.API.Services
{
    public class StudentService : IStudentService
    {
        private readonly AppDbContext _context;

        public StudentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<HomeScreenDto> GetHomeScreen(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new Exception("User not found");

            var analysis = await _context.CvAnalyses.Where(a => a.UserId == userId).OrderByDescending(a => a.AnalyzedAt).FirstOrDefaultAsync();

            var completedAssessments = await _context.Assessments.CountAsync(a => a.UserId == userId && a.Status == "completed");
            var completedCourses = await _context.Enrollments.CountAsync(e => e.UserId == userId && e.IsCompleted);
            var totalXp = (completedAssessments * 100) + (user.HasCv ? 50 : 0) + (completedCourses * 100);
            //var totalXp = (Assessment * 100) + (has cv ? 50 : 0) + (completedCourses * 200)

            var currentRand = totalXp >= 25000 ? "Master" :
                              totalXp >= 10000 ? "Expert Talent" :
                              totalXp >= 5000 ? "Verified Candidate" :
                              totalXp >= 1000 ? "Rising Talent" : "Scout";

            var myGroup = await _context.StudyGroupMembers.Include(m => m.StudyGroup).FirstOrDefaultAsync(m => m.UserId == userId);

            var weeklyplan = await GetWeeklyPlan(userId);
            var todayChallenge = await GetTodayChallenge(userId);

            return new HomeScreenDto
            {
                Name = user.Name,
                SkillScore = analysis?.Score ?? 0,
                CurrentRank = currentRand,
                XpThisWeek = completedAssessments * 10,
                TodaysChallenge = todayChallenge,
                WeeklyPlan = weeklyplan,
                MyStudyGroup = myGroup == null ? null : new StudyGroupDto
                {
                    Id = myGroup.StudyGroup.Id,
                    Name = myGroup.StudyGroup.Name,
                    Field = myGroup.StudyGroup.Field,
                    MemberCount = myGroup.StudyGroup.MemberCount,
                    ActiveCount = myGroup.StudyGroup.ActiveCount,
                    IsMember = true
                }
            };
        }

        public async Task<DailyChallengeDto> GetTodayChallenge(Guid userId)
        {
            var today = DateTime.UtcNow.Date;
            var challenge = await _context.DailyChallenges.FirstOrDefaultAsync(c => c.ChallengeDate == today && c.IsActive);

            if (challenge == null)
            {
                challenge = new DailyChallenge
                {
                    Id = Guid.NewGuid(),
                    Title = "Marketing Quiz #14",
                    Field = "Marketing",
                    QuestionCount = 10,
                    EstimatedMinutes = 8,
                    StudentsDoneToday = 63,
                    ChallengeDate = today,
                    IsActive = true
                };
                _context.DailyChallenges.Add(challenge);
                await _context.SaveChangesAsync();
            }

            return new DailyChallengeDto
            {
                Id = challenge.Id,
                Title = challenge.Title,
                Field = challenge.Field,
                QuestionCount = challenge.QuestionCount,
                EstimatedMinutes = challenge.EstimatedMinutes,
                StudentsDoneToday = challenge.StudentsDoneToday
            };
        }

        public async Task<WeeklyPlanProgressDto> GetWeeklyPlan(Guid userId)
        {
            var now = DateTime.UtcNow.Date;
            var weekNumber = System.Globalization.ISOWeek.GetWeekOfYear(now);

            var items = await _context.WeeklyPlans.Where(w => w.UserId == userId && w.WeekNumber == weekNumber && w.Year == now.Year).ToListAsync();

            if (!items.Any())
            {
                var defaultItems = new List<string> { "Chapter 4", "Chapter 5", "Chapter 6", "Weekly quiz" };
                foreach (var title in defaultItems)
                {
                    items.Add(new WeeklyPlan
                    {
                        Id = Guid.NewGuid(),
                        UserId = userId,
                        Title = title,
                        IsCompleted = false,
                        WeekNumber = weekNumber,
                        Year = now.Year
                    });
                }
                _context.WeeklyPlans.AddRange(items);
                await _context.SaveChangesAsync();
            }

            var completedCount = items.Count(i => i.IsCompleted);
            var progressPercent = items.Any() ? (int)Math.Round((double)completedCount / items.Count * 100) : 0;

            return new WeeklyPlanProgressDto
            {
                ProgressPercent = progressPercent,
                Items = items.Select(i => new WeeklyPlanDto
                {
                    Id = Guid.NewGuid(),
                    Title = i.Title,
                    IsCompleted = i.IsCompleted
                }).ToList()
            };
        }
    }
}