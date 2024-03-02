using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealTimeCollaborativeWhiteboard.Data;
using RealTimeCollaborativeWhiteboard.Models;
using System.Security.Claims;

namespace RealTimeCollaborativeWhiteboard.Controllers
{
    public class UploadImagesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public UploadImagesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var image = _dbContext.Images.ToList();
            return View(image);
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

                var file = new Image
                {
                    UrlImage = uniqueFileName,
                    CurrUserID = userId
                };

                _dbContext.Images.Add(file);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View();
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
            var image = await _dbContext.Images.FirstOrDefaultAsync(f => f.ImageId == id);
            if (image != null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (image.CurrUserID == userId)
                {
                    if (!string.IsNullOrEmpty(image.UrlImage))
                    {
                        var filePath = Path.Combine("Data", "Photos", image.UrlImage);
                        if (System.IO.File.Exists(filePath))
                        {
                            try
                            {
                                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                                {
                                    System.IO.File.Delete(filePath);
                                }
                            }
                            catch (IOException ex)
                            {}
                        }
                    }
                }
                _dbContext.Images.Remove(image);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
