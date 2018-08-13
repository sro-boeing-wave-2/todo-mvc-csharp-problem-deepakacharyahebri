using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ToDoAssignment.Models
{
    public class ToDoContext : DbContext
    {
        public ToDoContext (DbContextOptions<ToDoContext> options)
            : base(options)
        {
        }

        public DbSet<ToDoAssignment.Models.Note> Notes { get; set; }
        public DbSet<ToDoAssignment.Models.Label> Labels { get; set; }
        public DbSet<ToDoAssignment.Models.CheckList> CheckList { get; set; }
    }
}
