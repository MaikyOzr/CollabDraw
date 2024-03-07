using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealTimeCollaborativeWhiteboard.Data;
using RealTimeCollaborativeWhiteboard.Models;
using RealTimeCollaborativeWhiteboard.Services;
using System.Security.Claims;

namespace RealTimeCollaborativeWhiteboard.Controllers
{
    public class PDFController : Controller, IFileStorage
    {
        private readonly ApplicationDbContext _dbContext;

        public PDFController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var pdf = _dbContext.PDF.ToList();
            return View(pdf);
        }

        [HttpPost]
        [Authorize]
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        public async Task<IActionResult> DeleteFile(int id)
        {
            var pdf = await _dbContext.PDF.FirstOrDefaultAsync(f => f.PDFId == id);
            if (pdf != null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (pdf.CurrUserID == userId)
                {
                    if (!string.IsNullOrEmpty(pdf.PDFUrl))
                    {
                        var filePath = Path.Combine("Data", "PDF", pdf.PDFUrl);

                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }
                _dbContext.PDF.Remove(pdf);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetFile(string fileName)
        {
            var filePath = Path.Combine("Data", "PDFs", fileName);
            if (System.IO.File.Exists(filePath))
            {
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return new FileContentResult(fileBytes, "application/pdf")
                {
                    FileDownloadName = fileName
                };
            }
            else
            {
                return NotFound();
            }

        }

        [HttpPost]
        [Authorize]
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        public async Task<IActionResult> SaveFile(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Data", "PDF");
                var uniquePDFName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploadFolder, uniquePDFName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var pdf = new PDF
                {
                    PDFUrl = uniquePDFName,
                    CurrUserID = userId
                };

                _dbContext.PDF.Add(pdf);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View("Index");

        }
    }
}
