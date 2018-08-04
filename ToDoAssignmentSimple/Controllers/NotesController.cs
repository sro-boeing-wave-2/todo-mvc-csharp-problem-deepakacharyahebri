using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoAssignmentSimple.Models;

namespace ToDoAssignmentSimple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly ToDoContext _context;

        public NotesController(ToDoContext context)
        {
            _context = context;
        }

        // GET: api/Notes
        [HttpGet]
        public IEnumerable<Note> GetNote()
        {
            return _context.Note.Include(s => s.Labels).Include(y => y.CheckLists);
        }

        // GET: api/Notes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNote([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var note = await _context.Note.Include(s => s.Labels).Include(y => y.CheckLists).SingleOrDefaultAsync(x => x.Id ==id);

            if (note == null)
            {
                return NotFound();
            }

            return Ok(note);
        }

        [HttpGet("search/{title}")]
        public IEnumerable<Note> SearchByTiitel([FromRoute] string title)
        {
            var x =  _context.Note.Include(s => s.Labels).Include(s => s.CheckLists).Where(s => s.Title.Contains(title));
            return x;
        }

        // get by title
        [HttpGet("title/{title}")]
        public IEnumerable<Note> SearchByTitle([FromRoute] string title)
        {
            return _context.Note.Include(s => s.Labels).Include(s => s.CheckLists).Where(s=>s.Title==title);
        }

        [HttpGet("label/{label}")]
        public IActionResult SearchByLabel([FromRoute] string label)
        {
            //throw new Exception("Not Implemented");
            var NonNullDatas =  _context.Note.Include(s => s.CheckLists).Include(s => s.Labels).Where(x=>x.Labels != null);
            return Ok(NonNullDatas.Where(x => x.Labels.Any(y => y.LabelData == label)));

        }
        [HttpGet("pinned")]
        public IEnumerable<Note> PinnedNotes()
        {
            return _context.Note.Include(s => s.Labels).Include(s => s.CheckLists).Where(s => s.PinStatus==true);
        }

        // PUT: api/Notes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNote([FromRoute] int id, [FromBody] Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != note.Id)
            {
                return BadRequest();
            }
            //var Notes = _context.Note.Include(s => s.Labels).Include(y => y.CheckLists);
            //_context.Entry(note).State = EntityState.Modified;
            _context.Note.Update(note);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Notes
        [HttpPost]
        public async Task<IActionResult> PostNote([FromBody] Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Note.Add(note);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNote", new { id = note.Id }, note);
        }

        // DELETE: api/Notes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var note = await _context.Note.FindAsync(id);
            var note = await _context.Note.Include(s => s.Labels).Include(s => s.CheckLists).SingleOrDefaultAsync(s => s.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            _context.Note.Remove(note);
            await _context.SaveChangesAsync();

            return Ok(note);
        }

        [HttpDelete("deletelabel/{Label}")]
        public async Task<IActionResult> DeleteNoteByLabel([FromRoute] string Label)
        {
            /*if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/

            //var note = await _context.Note.FindAsync(id);
            var NonNullDatas = _context.Note.Include(s => s.CheckLists).Include(s => s.Labels).Where(x => x.Labels != null);
            var Notes = NonNullDatas.Where(x => x.Labels.Any(v => v.LabelData == Label));
            _context.Note.RemoveRange(Notes);

            await _context.SaveChangesAsync();

            return Ok(Notes);
        }

        private bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.Id == id);
        }
    }
}