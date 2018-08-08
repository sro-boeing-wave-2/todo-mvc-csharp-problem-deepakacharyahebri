using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoAssignment.Models;

namespace ToDoAssignment.Controllers
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
        public async Task<IEnumerable<Notes>> GetNotes()
        {
            var AllNotes = await _context.Notes.Include(s => s.Labels).Include(y => y.CheckLists).ToListAsync();
            return AllNotes;
        }

        // GET: api/Notes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNote([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var note = await _context.Notes.Include(s => s.Labels).Include(y => y.CheckLists).SingleOrDefaultAsync(x => x.Id == id);

            if (note == null)
            {
                return NotFound();
            }

            return Ok(note);
        }

        [HttpGet("search/{title}")]
        public async Task<IEnumerable<Notes>> SearchByTitlee([FromRoute] string title)
        {
            var SearchByTitleResults = await _context.Notes.Include(s => s.Labels).Include(s => s.CheckLists).Where(s => s.Title.Contains(title)).ToListAsync();
            return SearchByTitleResults;
        }

        // get by title
        [HttpGet("title/{title}")]
        public async Task<IEnumerable<Notes>> SearchByTitle([FromRoute] string title)
        {
            var FindByTitleResults = await _context.Notes.Include(s => s.Labels).Include(s => s.CheckLists).Where(s => s.Title == title).ToListAsync();
            return FindByTitleResults;
        }

        [HttpGet("label/{label}")]
        public async Task<IActionResult> SearchByLabel([FromRoute] string label)
        {
            //throw new Exception("Not Implemented");
            var NonNullDatas = _context.Notes.Include(s => s.CheckLists).Include(s => s.Labels).Where(x => x.Labels != null);
            return Ok(await NonNullDatas.Where(x => x.Labels.Any(y => y.LabelData == label)).ToListAsync());

        }
        [HttpGet("pinned")]
        public async Task<IActionResult> PinnedNotes()
        {
            var Pinned = await _context.Notes.Include(s => s.Labels).Include(s => s.CheckLists).Where(s => s.PinStatus == true).ToListAsync();
            return Ok(Pinned);
        }

        // PUT: api/Notes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNote([FromRoute] int id, [FromBody] Notes note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != note.Id)
            {
                return BadRequest();
            }
            //var Notes = _context.Notes.Include(s => s.Labels).Include(y => y.CheckLists);
            //_context.Entry(note).State = EntityState.Modified;
            _context.Notes.Update(note);
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
        public async Task<IActionResult> PostNote([FromBody] Notes note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Notes.Add(note);
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

            //var note = await _context.Notes.FindAsync(id);
            var note = await _context.Notes.Include(s => s.Labels).Include(s => s.CheckLists).SingleOrDefaultAsync(s => s.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            _context.Notes.Remove(note);
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

            //var note = await _context.Notes.FindAsync(id);
            var NonNullDatas = _context.Notes.Include(s => s.CheckLists).Include(s => s.Labels).Where(x => x.Labels != null);
            var Notes = NonNullDatas.Where(x => x.Labels.Any(v => v.LabelData == Label));
            _context.Notes.RemoveRange(Notes);

            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool NoteExists(int id)
        {
            return _context.Notes.Any(e => e.Id == id);
        }
        /*private readonly ToDoContext _context;

        public NotesController(ToDoContext context)
        {
            _context = context;
        }

        // GET: api/Notes
        [HttpGet]
        public IEnumerable<Notes> GetNotes()
        {
            return _context.Notes;
        }

        // GET: api/Notes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotes([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notes = await _context.Notes.FindAsync(id);

            if (notes == null)
            {
                return NotFound();
            }

            return Ok(notes);
        }

        // PUT: api/Notes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotes([FromRoute] int id, [FromBody] Notes notes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != notes.Id)
            {
                return BadRequest();
            }

            _context.Entry(notes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotesExists(id))
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
        public async Task<IActionResult> PostNotes([FromBody] Notes notes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Notes.Add(notes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNotes", new { id = notes.Id }, notes);
        }

        // DELETE: api/Notes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotes([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notes = await _context.Notes.FindAsync(id);
            if (notes == null)
            {
                return NotFound();
            }

            _context.Notes.Remove(notes);
            await _context.SaveChangesAsync();

            return Ok(notes);
        }

        private bool NotesExists(int id)
        {
            return _context.Notes.Any(e => e.Id == id);
        }*/
    }
}