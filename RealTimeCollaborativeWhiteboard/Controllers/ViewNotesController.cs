using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealTimeCollaborativeWhiteboard.Data;
using RealTimeCollaborativeWhiteboard.Models;
using System.Composition;
using System.Security.Claims;

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
        [Authorize]
        public ActionResult Index()
        {
            var notes = _dbContext.Notes.ToList();
            return View(notes);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                var note = await _dbContext.Notes.FirstOrDefaultAsync(p => p.NotesId == id);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (note.CurrUserID == userId)
                {
                    if (note != null)
                    {
                        _dbContext.Notes.Remove(note);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Notes note = await _dbContext.Notes.FindAsync(id);
            if (note == null)
            {
                return RedirectToAction("Index");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (note.CurrUserID != userId) {
                return RedirectToAction("Index");
            }

            return View(note);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id, Notes updateNote)
        {
            var note = await _dbContext.Notes.FirstOrDefaultAsync(p => p.NotesId == id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (note != null)
            {
                if(note.CurrUserID == userId) {
                    if (updateNote.Title != null && updateNote.Content != null)
                    {
                        note.Title = updateNote.Title;
                        note.Content = updateNote.Content;
                    }
                }
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Edit");
        }
    }
}
