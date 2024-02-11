using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealTimeCollaborativeWhiteboard.Data;
using RealTimeCollaborativeWhiteboard.Models;

namespace RealTimeCollaborativeWhiteboard.Controllers
{
    public class ViewDeskController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _dbContext;

        public ViewDeskController(IWebHostEnvironment environment, ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _environment = environment;
        }


        [HttpGet]
        public IActionResult Index()
        {
            var photoPath = Path.Combine(_environment.WebRootPath, "Photos");
            var photoFiles = Directory.GetFiles(photoPath);
            return View(photoFiles);
        }
    }
}
