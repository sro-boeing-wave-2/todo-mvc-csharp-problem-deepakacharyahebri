using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoAssignmentSimple.Models;

namespace ToDoAssignmentSimple.Services
{
    public interface IToDoNoteService
    {
        IEnumerable<Note> GetAll();
        Note Get(int id);
        Note Add(Note note);
        void Update(int id, Note note);
        void Delete(int id);
    }
}
