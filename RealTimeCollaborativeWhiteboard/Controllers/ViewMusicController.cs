using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealTimeCollaborativeWhiteboard.Data;

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
    }
}
