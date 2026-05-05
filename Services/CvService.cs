using Microsoft.EntityFrameworkCore;
using RecruitmentApp.API.Data;
using RecruitmentApp.API.DTOs;
using RecruitmentApp.API.Models;

namespace RecruitmentApp.API.Services
{
    public class CvService : ICvService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CvService(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<CvUploadResponseDto> UploadCv(IFormFile file, Guid userId)
        {
            if (file == null || file.Length == 0) throw new Exception("No File Uploaded");

            var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension)) throw new Exception("Only PDF and Word Documents are allowed");

            var uploadsFolder = Path.Combine(_environment.ContentRootPath, "Uploads", "CVs");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{userId}_{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var cvUpload = new CvUpload
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                FileName = file.FileName,
                FilePath = filePath,
                Status = "uploaded",
                UploadedAt = DateTime.UtcNow
            };

            _context.CvUpload.Add(cvUpload);
            await _context.SaveChangesAsync();

            return new CvUploadResponseDto
            {
                CvId = cvUpload.Id,
                FileName = file.FileName,
                Status = "uploaded",
                Message = "CV uploaded successfully, AI analysis in progress"
            };
        }
    }
}