namespace RecruitmentApp.API.DTOs
{
    public class FieldWithSpecializationsDto
    {
        public string Name { get; set; } = string.Empty;
        public List<string> Specializations { get; set; } = new();
    }

    public class SetupOptionsDto
    {
        public List<FieldWithSpecializationsDto> Fields { get; set; } = new();
        public List<string> ExperienceLevels { get; set; } = new();
    }
}