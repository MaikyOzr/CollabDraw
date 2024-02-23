using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealTimeCollaborativeWhiteboard.Data;
using RealTimeCollaborativeWhiteboard.Models;
using System.Composition;

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
            var notes = _dbContext.Notes.ToList();
            return View(notes);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                var note = await _dbContext.Notes.FirstOrDefaultAsync(p => p.NotesId == id);
                if (note != null)
                {
                    _dbContext.Notes.Remove(note);
                    await _dbContext.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
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

            return View(note);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Notes updateNote)
        {
            var note = await _dbContext.Notes.FirstOrDefaultAsync(p => p.NotesId == id);
            if (note != null)
            {
                if (updateNote.Title != null)
                {
                    note.Title = updateNote.Title;
                }
                if (updateNote.Content != null)
                {
                    note.Content = updateNote.Content;
                }
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Edit");
        }




    }
}
