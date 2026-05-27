namespace RecruitmentApp.API.DTOs
{
    public class SetupOptionsDto
    {
        public List<string> Fields { get; set; } = new();
        public Dictionary<string, List<string>> Specializations { get; set; } = new();
        public List<string> ExperienceLevels { get; set; } = new();
    }
}