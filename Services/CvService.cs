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

            //var uploadsFolder = Path.Combine(_environment.ContentRootPath, "Uploads", "CVs");
            //if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            //var uniqueFileName = $"{userId}_{Guid.NewGuid()}{fileExtension}";
            //var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            //using (var stream = new FileStream(filePath, FileMode.Create))
            //{
            //    await file.CopyToAsync(stream);
            //}

            var uniqueFileName = $"{userId}_{Guid.NewGuid()}{fileExtension}";
            var filePath = uniqueFileName; 

            var cvUpload = new CvUpload
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                FileName = file.FileName,
                FilePath = filePath,
                Status = "uploaded",
                UploadedAt = DateTime.UtcNow
            };

            _context.CvUploads.Add(cvUpload);
            await _context.SaveChangesAsync();

            /*try
            {
                _context.CvUploads.Add(cvUpload);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.InnerException?.Message
                                ?? ex.InnerException?.Message
                                ?? ex.Message;
                throw new Exception(innerMessage);
            }
            */

            var analysis = GenerateMockAnalysis(cvUpload.Id, userId);
            _context.CvAnalyses.Add(analysis);
            cvUpload.Status = "analyzed";
            await _context.SaveChangesAsync();

            /*try
            {
                var analysis = GenerateMockAnalysis(cvUpload.Id, userId);
                _context.CvAnalyses.Add(analysis);
                cvUpload.Status = "analyzed";
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.InnerException?.Message
                                ?? ex.InnerException?.Message
                                ?? ex.Message;
                throw new Exception(innerMessage);
            }*/

            return new CvUploadResponseDto
            {
                CvId = cvUpload.Id,
                FileName = file.FileName,
                Status = "uploaded",
                Message = "CV uploaded successfully, AI analysis in progress"
            };
        }

        private CvAnalysis GenerateMockAnalysis(Guid cvId, Guid userId)
        {
            var mockSkills = new List<string> { "Communication", "Problem Solving", "Teamwork" };
            var mockGaps = new List<string> { "Docker", "System Design", "Cloud Computing" };

            return new CvAnalysis
            {
                Id = Guid.NewGuid(),
                CvId = cvId,
                UserId = userId,
                Skills = System.Text.Json.JsonSerializer.Serialize(mockSkills),
                Gaps = System.Text.Json.JsonSerializer.Serialize(mockGaps),
                Score = new Random().Next(50, 90),
                ExperienceLevel = "Junior",
                Field = "Software Development",
                AnalyzedAt = DateTime.UtcNow
            };
        }

        public async Task<CvAnalysisResponseDto> GetCvAnalysis(Guid userId)
        {
            var analysis = await _context.CvAnalyses.Where(a => a.UserId == userId).OrderByDescending(a => a.AnalyzedAt).FirstOrDefaultAsync();

            if (analysis == null) throw new Exception("No CV analysis found");

            return new CvAnalysisResponseDto
            {
                CvId = analysis.CvId,
                Skills = System.Text.Json.JsonSerializer.Deserialize<List<string>>(analysis.Skills) ?? new(),
                Gaps = System.Text.Json.JsonSerializer.Deserialize<List<string>>(analysis.Gaps) ?? new(),
                Score = analysis.Score,
                ExperienceLevel = analysis.ExperienceLevel,
                Field = analysis.Field,
                Specialization = analysis.Specialization ?? string.Empty
            };
        }
    }
}