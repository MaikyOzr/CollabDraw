using Microsoft.AspNetCore.Mvc;
using RealTimeCollaborativeWhiteboard.Data;
using RealTimeCollaborativeWhiteboard.Models;


namespace RealTimeCollaborativeWhiteboard.Controllers
{
    public class CreateDeskController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _dbContext;

        public CreateDeskController(IWebHostEnvironment environment, ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _environment = environment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        public async Task<IActionResult> SavePhoto(IFormFile photoFile)
        {
            if (photoFile != null && photoFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "Photos");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + photoFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await photoFile.CopyToAsync(fileStream);
                }

                var desk = new Desk
                {
                    UrlPhoto = uniqueFileName 
                };

                // Save to database
                _dbContext.Desks.Add(desk);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View("Create"); 
        }
    }
}

