using Microsoft.EntityFrameworkCore;
using RecruitmentApp.API.Data;
using RecruitmentApp.API.DTOs;
using RecruitmentApp.API.Models;

namespace RecruitmentApp.API.Services
{
    public class AssessmentService : IAssessmentService
    {
        private readonly AppDbContext _context;

        public AssessmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AssessmentSessionDto> StartAssessment(Guid userId, StartAssessmentDto dto)
        {
            var questions = await GetorCreateQuestions(dto.Field);
            var assessment = new Assessment
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Field = dto.Field,
                Status = "in-progress",
                TotalQuestions = questions.Count,
                StartedAt = DateTime.UtcNow
            };

            _context.Assessments.Add(assessment);
            await _context.SaveChangesAsync();

            return new AssessmentSessionDto
            {
                SessionId = assessment.Id,
                Field = dto.Field,
                TimeLimitMinutes = 20,
                Questions = questions.Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Text = q.Text,
                    Options = System.Text.Json.JsonSerializer.Deserialize<List<string>>(q.Options) ?? new()
                }).ToList()
            };
        }

        public async Task<AssessmentResultDto> SubmitAssessment(Guid userId, Guid sessionId, SubmitAssessmentDto dto)
        {
            var assessment = await _context.Assessments.FirstOrDefaultAsync(a => a.Id == sessionId && a.UserId == userId);

            if (assessment == null)
                throw new Exception("Assessment session in not found");
            if (assessment.Status == "completed")
                throw new Exception("Assessment already submitted");

            var questionIds = dto.Answers.Select(a => a.QuestionId).ToList();
            var questions = await _context.Questions.Where(q => questionIds.Contains(q.Id)).ToListAsync();

            int correctCount = 0;
            foreach (var answer in dto.Answers)
            {
                var question = questions.FirstOrDefault(q => q.Id == answer.QuestionId);
                if (questions != null && question.CorrectOptionIndex == answer.SelectedOptionIndex)
                    correctCount++;
            }

            int score = (int)Math.Round((double)correctCount / assessment.TotalQuestions * 100);

            assessment.Score = score;
            assessment.CorrectAnswers = correctCount;
            assessment.Status = "completed";
            assessment.CompletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var level = score >= 80 ? "Senior" : score >= 60 ? "Mid" : "Junior";

            return new AssessmentResultDto
            {
                SessionId = sessionId,
                Score = score,
                TotalQuestions = assessment.TotalQuestions,
                CorrectAnswers = correctCount,
                MissedAnswers = assessment.TotalQuestions - correctCount,
                Level = level,
                Strengths = new List<string> { "Problem Solving", "Communication" },
                Weaknesses = new List<string> { "Docker", "System Design" },
                RecommendedCourse = score < 60 ? "Master React Hooks" : "Advanced System Design"
            };
        }

        public async Task<AssessmentMainDto> GetAssessmentMain(Guid userId)
        {
            var completed = await _context.Assessments
                .Where(a => a.UserId == userId && a.Status == "completed")
                .ToListAsync();

            var avgScore = completed.Any() ? (int)completed.Average(a => a.Score) : 0;
            var badges = completed.Count(a => a.Score >= 80);

            var rank = avgScore >= 85 ? "Top 15%" : avgScore >= 70 ? "Top 30%" : "Top 50%";

            return new AssessmentMainDto
            {
                CompletedCount = completed.Count,
                AvgScore = avgScore,
                Badges = badges,
                Rank = rank,
                Recommended = new List<RecommendedAssessmentDto>
        {
            new() { Title = "React.js Advanced", Priority = "HIGH", Duration = "25 mins", XpReward = 60 },
            new() { Title = "System Design", Priority = "MEDIUM", Duration = "30 mins", XpReward = 80 }
        },
                Available = new List<AvailableAssessmentDto>
        {
            new() { Field = "UI/UX Principles", Duration = "20 mins", XpReward = 40 },
            new() { Field = "Python Fundamentals", Duration = "30 mins", XpReward = 60 },
            new() { Field = "Product Management", Duration = "45 mins", XpReward = 80 }
        }
            };
        }

        public async Task<List<AssessmentHistoryDto>> GetTestHistory(Guid userId)
        {
            var assessments = await _context.Assessments
                .Where(a => a.UserId == userId && a.Status == "completed")
                .OrderByDescending(a => a.CompletedAt)
                .ToListAsync();

            return assessments.Select(a => new AssessmentHistoryDto
            {
                Id = a.Id,
                Field = a.Field,
                Specialization = a.Specialization,
                Score = a.Score,
                Status = a.Score >= 80 ? "Expert" : a.Score >= 65 ? "Qualified" : a.Score >= 50 ? "Developing" : "Gap Identified",
                CompletedAt = a.CompletedAt ?? DateTime.UtcNow
            }).ToList();
        }

        private async Task<List<Question>> GetorCreateQuestions(string field)
        {
            var existing = await _context.Questions.Where(q => q.Field == field).Take(20).ToListAsync();

            if (existing.Any()) return existing;

            var mockQuestions = new List<Question>
            {
                new Question { Id = Guid.NewGuid(), Field = field, Text = "What does useEffect do when its dependency array is empty?", Options = System.Text.Json.JsonSerializer.Serialize(new List<string> { "Runs on every render", "Runs once after initial render", "Never runs", "Crashes the app" }), CorrectOptionIndex = 1 },
                new Question { Id = Guid.NewGuid(), Field = field, Text = "Which hook is used for state management in React?", Options = System.Text.Json.JsonSerializer.Serialize(new List<string> { "useEffect", "useState", "useContext", "useRef" }), CorrectOptionIndex = 1 },
                new Question { Id = Guid.NewGuid(), Field = field, Text = "What is the purpose of the virtual DOM?", Options = System.Text.Json.JsonSerializer.Serialize(new List<string> { "Direct DOM manipulation", "Performance optimization", "State management", "Routing" }), CorrectOptionIndex = 1 },
                new Question { Id = Guid.NewGuid(), Field = field, Text = "What does REST stand for?", Options = System.Text.Json.JsonSerializer.Serialize(new List<string> { "Remote Execution State Transfer", "Representational State Transfer", "Request State Transfer", "None of the above" }), CorrectOptionIndex = 1 },
                new Question { Id = Guid.NewGuid(), Field = field, Text = "Which HTTP method is used to update a resource?", Options = System.Text.Json.JsonSerializer.Serialize(new List<string> { "GET", "POST", "PUT", "DELETE" }), CorrectOptionIndex = 2 }
            };

            _context.Questions.AddRange(mockQuestions);
            _context.SaveChanges();

            return mockQuestions;
        }
    }
}