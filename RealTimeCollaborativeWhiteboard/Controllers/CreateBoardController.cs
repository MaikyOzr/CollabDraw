using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealTimeCollaborativeWhiteboard.Data;
using RealTimeCollaborativeWhiteboard.Models;

namespace RealTimeCollaborativeWhiteboard.Controllers
{
    public class CreateBoardController : Controller
    {

        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _dbContext;

        public CreateBoardController(IWebHostEnvironment environment, ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _environment = environment;
        }

        [HttpGet]
        public ActionResult CreateBoard()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateBoard(Board board) {
            
            return View();
        }
        
    }
}
