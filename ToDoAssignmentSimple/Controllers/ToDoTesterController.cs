using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoAssignmentSimple.Models;
using ToDoAssignmentSimple.Services;

namespace ToDoAssignmentSimple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoTesterController : ControllerBase
    {
        private readonly ToDoTesterContext _context;
        private IToDoNoteService _todonoteservices;

        public ToDoTesterController(IToDoNoteService todonoteservices)
        {
            _todonoteservices = todonoteservices;
        }

        // GET: api/ToDoTester
        [HttpGet]
        public IActionResult GetNote()
        {
            var models = _todonoteservices.GetAll();
            return Ok(models);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var model = _todonoteservices.Get(id);

            return Ok(model);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var person = _todonoteservices.Add(note);

            return CreatedAtAction("Get", new { id = person.Id }, person);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _todonoteservices.Update(id, note);

            return NoContent();
        }
        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _todonoteservices.Delete(id);
            return NoContent();
        }



        // GET: api/ToDoTester/5
        /*[HttpGet("{id}")]
        public async Task<IActionResult> GetNote([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var note = await _context.Note.FindAsync(id);

            if (note == null)
            {
                return NotFound();
            }

            return Ok(note);
        }*/

        // PUT: api/ToDoTester/5
        /*[HttpPut("{id}")]
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

            _context.Entry(note).State = EntityState.Modified;

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

        // POST: api/ToDoTester
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

        // DELETE: api/ToDoTester/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var note = await _context.Note.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            _context.Note.Remove(note);
            await _context.SaveChangesAsync();

            return Ok(note);
        }*/

        private bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.Id == id);
        }
    }
}