using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealTimeCollaborativeWhiteboard.Data;
using RealTimeCollaborativeWhiteboard.Models;
using System.Security.Claims;

namespace RealTimeCollaborativeWhiteboard.Controllers
{
    public class CreateNotesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CreateNotesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult CreateNotes()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateNotes(Notes note)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            note.CurrUserID = userId;
            _dbContext.Notes.Add(note);
            await _dbContext.SaveChangesAsync();


            return RedirectToAction("Index", "ViewNotes");
        }

    }
}
