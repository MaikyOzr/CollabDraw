using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealTimeCollaborativeWhiteboard.Data;
using RealTimeCollaborativeWhiteboard.Models;
using System.Security.Claims;

namespace RealTimeCollaborativeWhiteboard.Controllers
{
    public class UploadDocFilesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public UploadDocFilesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index() {
            var doc = _dbContext.DocFiles.ToList();
            return View(doc);
        }

        [HttpPost]
        [Authorize]
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        public async Task<IActionResult> SaveDocFile(IFormFile docFile) {
            if (docFile != null && docFile.Length > 0) {
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Docs");
                var uniqueDocName = Guid.NewGuid().ToString() + "_"+docFile.FileName;
                var filePath = Path.Combine(uploadFolder, uniqueDocName);

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                using (var fileStream = new FileStream(filePath, FileMode.Create)) {
                    await docFile.CopyToAsync(fileStream);
                }

                var docFiles = new DocFiles
                {
                    DocUrl = uniqueDocName,
                    CurrUserID = userId,
                };

                _dbContext.DocFiles.Add(docFiles);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        [HttpGet]
        public IActionResult GetDocuments(string fileName)
        {
            var filePath = Path.Combine("Data", "Docs", fileName);
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "application/msword");
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteDocFile(int id)
        {
            var docFile = await _dbContext.DocFiles.FirstOrDefaultAsync(d => d.DocId == id);
            if (docFile != null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (docFile.CurrUserID == userId)
                {
                    if (!string.IsNullOrEmpty(docFile.DocUrl))
                    {
                        var filePath = Path.Combine("Data", "Docs", docFile.DocUrl);

                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }
                _dbContext.DocFiles.Remove(docFile);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
