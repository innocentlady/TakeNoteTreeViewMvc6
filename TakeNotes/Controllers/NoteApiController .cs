using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TakeNotes.Data;
using TakeNotes.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TakeNotes.Controllers
{      [ApiController]
       [Route("api/notes")]
    public class NoteApiController : ControllerBase
    {
  
      
            private readonly NoteContext _context;

            public NoteApiController(NoteContext context)
            {
                _context = context;
            }
    /// <summary>
    /// Get All Notes
    /// </summary>
    /// <returns></returns>
        [HttpGet]
            public async Task<ActionResult<List<NoteModel>>> Get()
            {
                var notes = await _context.Notes.Where(n => !n.IsDeleted).ToListAsync();

                return Ok(notes);
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<NoteModel>> Get(int id)
            {
                var note = await _context.Notes.FindAsync(id);

                if (note == null || note.IsDeleted)
                {
                    return NotFound();
                }

                return Ok(note);
            }

        [HttpPost]
        public async Task<ActionResult<NoteModel>> Post(NoteModel note)
        {
            if (note == null || string.IsNullOrWhiteSpace(note.Title) || string.IsNullOrWhiteSpace(note.Body))
            {
                return BadRequest("Title and body fields are required.");
            }

            note.CreatedAt = DateTime.UtcNow;
            note.UpdatedAt = null;
            note.IsDeleted = false;

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = note.Id }, note);
        }

        [HttpPut("{id}")]
            public async Task<IActionResult> Put(int id, NoteModel note)
            {
                var existingNote = await _context.Notes.FindAsync(id);

                if (existingNote == null || existingNote.IsDeleted)
                {
                    return NotFound();
                }

                if (note == null || string.IsNullOrWhiteSpace(note.Title))
                {
                    return BadRequest("Invalid note.");
                }

                existingNote.Title = note.Title;
                existingNote.Body = note.Body;
                existingNote.ParentId = note.ParentId;
                existingNote.UpdatedAt = DateTime.UtcNow;
                existingNote.CreatedAt = DateTime.UtcNow;
                 existingNote.IsDeleted = false;

                await _context.SaveChangesAsync();

                return NoContent();
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                var existingNote = await _context.Notes.FindAsync(id);

                if (existingNote == null || existingNote.IsDeleted)
                {
                    return NotFound();
                }

                existingNote.IsDeleted = true;

                await _context.SaveChangesAsync();

                return NoContent();
            }
        }
    }


