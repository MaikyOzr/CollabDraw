using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealTimeCollaborativeWhiteboard.Data;
using System.Security.Claims;

namespace RealTimeCollaborativeWhiteboard.Controllers
{
    public class ViewMusicController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ViewMusicController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            var music = _dbContext.Music.ToList();
            return View(music);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteMusic(int id) {

            var audio = await _dbContext.Music.FirstOrDefaultAsync(m => m.MusicId == id);

            if (audio != null) {

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (audio.CurrUserID == userId) {

                    if (!string.IsNullOrEmpty(audio.UrlMusic)) {

                        var filePath = Path.Combine("Data", "Music", audio.UrlMusic);

                        if (System.IO.File.Exists(filePath)) {

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
