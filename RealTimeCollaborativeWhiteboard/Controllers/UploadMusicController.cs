using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var audio = _dbContext.Music.ToList();
            return View(audio);
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

            return View("Index");
        }

        [HttpGet]
        public IActionResult GetMusic(string fileName)
        {
            var filePath = Path.Combine("Data", "Music", fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return File(fileStream, "audio/mpeg");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteMusic(int id)
        {
            var audio = await _dbContext.Music.FirstOrDefaultAsync(f => f.MusicId == id);
            if (audio != null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (audio.CurrUserID == userId)
                {
                    if (!string.IsNullOrEmpty(audio.UrlMusic))
                    {
                        var filePath = Path.Combine("Data", "Music", audio.UrlMusic);

                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }
                _dbContext.Music.Remove(audio);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

    }
}
