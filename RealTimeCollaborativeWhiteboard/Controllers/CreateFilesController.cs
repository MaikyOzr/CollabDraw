using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealTimeCollaborativeWhiteboard.Data;
using RealTimeCollaborativeWhiteboard.Models;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RealTimeCollaborativeWhiteboard.Controllers
{
    public class CreateFilesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CreateFilesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var desks = _dbContext.Files.ToList();
            return View(desks);
        }

        [HttpPost]
        [Authorize]
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        public async Task<IActionResult> SavePhoto(IFormFile photoFile)
        {
            if (photoFile != null && photoFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Photos");
                var uniqueFileName = Path.GetRandomFileName() + Path.GetExtension(photoFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await photoFile.CopyToAsync(fileStream);
                }

                var file = new Files
                {
                    UrlPhoto = uniqueFileName,
                    CurrUserID = userId
                };

                _dbContext.Files.Add(file);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View("Index");
        }

        [HttpGet]
        public IActionResult GetPhotos(string fileName)
        {
            var filePath = Path.Combine("Data", "Photos", fileName);
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "image/jpeg");
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            var image = await _dbContext.Files.FirstOrDefaultAsync(f => f.DeskID == id);
            if (image != null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (image.CurrUserID == userId)
                {
                    if (!string.IsNullOrEmpty(image.UrlPhoto))
                    {
                        var filePath = Path.Combine("Data", "Photos", image.UrlPhoto);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }
                _dbContext.Files.Remove(image);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
