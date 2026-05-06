namespace RecruitmentApp.API.DTOs
{
    public class CvUploadResponseDto
    {
        public Guid CvId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}