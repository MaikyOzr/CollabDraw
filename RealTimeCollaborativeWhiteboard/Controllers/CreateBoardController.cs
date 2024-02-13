using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealTimeCollaborativeWhiteboard.Data;
using RealTimeCollaborativeWhiteboard.Models;

namespace RealTimeCollaborativeWhiteboard.Controllers
{
    public class CreateBoardController : Controller
    {

        //private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _dbContext;

        public CreateBoardController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult CreateBoard()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateBoard(Board board) {
            _dbContext.Boards.Add(board);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("CreateBoard");
        }
        
    }
}
