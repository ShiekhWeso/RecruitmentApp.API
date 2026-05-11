namespace RecruitmentApp.API.DTOs
{
    public class StartAssessmentDto
    {
        public string Field { get; set; } = string.Empty;
    }

    public class QuestionDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new();
    }

    public class AssessmentSessionDto
    {
        public Guid SessionId { get; set; }
        public string Field { get; set; } = string.Empty;
        public List<QuestionDto> Questions { get; set; } = new();
        public int TimeLimitMinutes { get; set; } = 20;
    }

    public class SubmitAnswerDto
    {
        public Guid QuestionId { get; set; }
        public int SelectedOptionIndex { get; set; }
    }

    public class SubmitAssessmentDto
    {
        public List<SubmitAnswerDto> Answers { get; set; } = new();
    }

    public class AssessmentResultDto
    {
        public Guid SessionId { get; set; }
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public int MissedAnswers { get; set; }
        public string Level { get; set; } = string.Empty;
        public List<string> Strengths { get; set; } = new();
        public List<string> Weaknesses { get; set; } = new();
        public string RecommendedCourse { get; set; } = string.Empty;
    }
}