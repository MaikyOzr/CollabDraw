using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealTimeCollaborativeWhiteboard.Data;
using RealTimeCollaborativeWhiteboard.Models;

namespace RealTimeCollaborativeWhiteboard.Controllers
{
    public class ViewFilesController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _dbContext;

        public ViewFilesController(IWebHostEnvironment environment, ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var desks = _dbContext.Files.ToList(); 
            return View(desks);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            var image = await _dbContext.Files.FirstOrDefaultAsync(f => f.DeskID == id);
            if (image != null)
            {
                if (!string.IsNullOrEmpty(image.UrlPhoto))
                {
                    var filePath = Path.Combine(_environment.WebRootPath, "Photos", image.UrlPhoto);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _dbContext.Files.Remove(image);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
