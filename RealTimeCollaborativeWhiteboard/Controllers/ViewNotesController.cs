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
        public async Task<IActionResult> Edit(Notes note)
        {
            if (ModelState.IsValid)
            {
                var existNotes = new Notes()
                {
                    NotesId = note.NotesId,
                    Title = note.Title,
                    Content = note.Content,
                };
                _dbContext.Notes.Update(existNotes);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
                //var existingNote = await _dbContext.Notes.AsNoTracking().SingleOrDefaultAsync(n => n.NotesId == note.NotesId);
                //if (existingNote != null)
                //{
                //    existingNote.Title = note.Title;
                //    existingNote.Content = note.Content;
                //    await _dbContext.SaveChangesAsync();
                //    return RedirectToAction("Index");
                //}
                //else
                //{
                //    return NotFound(); // Якщо не знайдено існуючий запис
                //}
            }
            return View(note); // Повернути форму редагування з помилками, якщо дані недійсні
        }




    }
}
