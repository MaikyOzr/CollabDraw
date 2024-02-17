using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealTimeCollaborativeWhiteboard.Data;

namespace RealTimeCollaborativeWhiteboard.Controllers
{
    public class ViewNotesController : Controller
    {

        private readonly ApplicationDbContext _dbContext;

        public ViewNotesController( ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var boards = _dbContext.Notes.ToList();
            return View(boards);
        }
    }
}
