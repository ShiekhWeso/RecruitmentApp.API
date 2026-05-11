namespace RecruitmentApp.API.Models
{
    public class Question
    {
        public Guid Id { get; set; }
        public string Field { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string Options { get; set; } = string.Empty;
        public int CorrectOptionIndex { get; set; }
    }
}