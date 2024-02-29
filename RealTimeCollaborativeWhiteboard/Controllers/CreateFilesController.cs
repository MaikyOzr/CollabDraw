using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeCollaborativeWhiteboard.Data;
using RealTimeCollaborativeWhiteboard.Models;
using System.Security.Claims;


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
            return View();
        }
        [HttpGet]
        public IActionResult CreateFiles()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        public async Task<IActionResult> SavePhoto(IFormFile photoFile)
        {
            if (photoFile != null && photoFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Photos");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + photoFile.FileName;
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

                // Save to database
                _dbContext.Files.Add(file);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View("CreateFiles"); 
        }

        [HttpGet]
        public IActionResult GetPhotos(string fileName) {
            var filePath = Path.Combine("Data", "Photos", fileName);
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "image/jpj");
        }
    }

}

