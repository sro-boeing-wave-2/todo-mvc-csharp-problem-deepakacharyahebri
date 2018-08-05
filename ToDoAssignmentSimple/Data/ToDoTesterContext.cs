using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ToDoAssignmentSimple.Models
{
    public class ToDoTesterContext : DbContext
    {
        public ToDoTesterContext (DbContextOptions<ToDoTesterContext> options)
            : base(options)
        {
        }

        public DbSet<ToDoAssignmentSimple.Models.Note> Note { get; set; }
    }
}
