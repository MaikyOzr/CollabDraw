using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeCollaborativeWhiteboard.Data;
using RealTimeCollaborativeWhiteboard.Models;
using System.Security.Claims;

namespace RealTimeCollaborativeWhiteboard.Controllers
{
    public class UploadMusicController : Controller
    {

        private readonly ApplicationDbContext _dbContext;

        public UploadMusicController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Upload() {
            return View();
        }


        [HttpPost]
        [Authorize]
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        public async Task<IActionResult> SaveMusic(IFormFile musicFile)
        {
            if (musicFile != null && musicFile.Length > 0)
            {
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Music");
                var uniqueMusicName = Guid.NewGuid().ToString() + "_" + musicFile.FileName;
                var filePath = Path.Combine(uploadFolder, uniqueMusicName);

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await musicFile.CopyToAsync(fileStream);
                }
                var music = new Music
                {
                    UrlMusic = uniqueMusicName,
                    CurrUserID = userId
                };
                _dbContext.Music.Add(music);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View("Upload");
        }

        [HttpGet]
        public IActionResult GetMusic(string fileName)
        {
            var filePath = Path.Combine("Data", "Music", fileName);
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "audio/mpeg");
        }

    }
}
