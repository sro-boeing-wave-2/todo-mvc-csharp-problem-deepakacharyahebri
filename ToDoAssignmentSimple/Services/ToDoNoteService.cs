using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoAssignmentSimple.Models;
using GenFu;

namespace ToDoAssignmentSimple.Services
{
    public class ToDoNoteService : IToDoNoteService
    {
        private List<Note> Notes { get; set; }

        public ToDoNoteService()
        {
            var i = 0;
            Notes = A.ListOf<Note>(50);
            Notes.ForEach(note =>
            {
                i++;
                note.Id = i;
            });
        }

        public IEnumerable<Note> GetAll()
        {
            return Notes;
        }
        public Note Get(int id)
        {
            return Notes.First(_ => _.Id == id);
        }

        public Note Add(Note note)
        {
            var newid = Notes.OrderBy(_ => _.Id).Last().Id + 1;
            note.Id = newid;

            Notes.Add(note);

            return note;
        }
        public void Update(int id, Note note)
        {
            var existing = Notes.First(_ => _.Id == id);
            existing.Title = note.Title;
            existing.PlainText = note.PlainText;
            existing.PinStatus = note.PinStatus;
            existing.Labels = note.Labels;
            existing.CheckLists = note.CheckLists;
        }
        public void Delete(int id)
        {
            var existing = Notes.First(_ => _.Id == id);
            Notes.Remove(existing);
        }

    }
}
