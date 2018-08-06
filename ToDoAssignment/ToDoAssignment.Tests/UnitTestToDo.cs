using System;
using ToDoAssignment.Controllers;
using ToDoAssignment.Models;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System.Collections.Generic;

namespace ToDoAssignment.Tests
{
    public class UnitTestToDo
    {
        private NotesController _controller;
        public UnitTestToDo()
        {
            var dbOptionBuilder = new DbContextOptionsBuilder<ToDoContext>();
            dbOptionBuilder.UseInMemoryDatabase("TempDB");
            ToDoContext todocontext = new ToDoContext(dbOptionBuilder.Options);
            _controller = new NotesController(todocontext);
            PrepareData(todocontext);
        }
        public void PrepareData(ToDoContext todocontext)
        {
            var notes = new List<Notes>()
            {
                new Notes()
                {
                    Title="My First Note",
                    PlainText="This is my plaintext",
                    PinStatus=true,
                    CheckLists=new List<CheckList>()
                    {
                        new CheckList()
                        {
                            CheckListData="checklist data 1",
                            ChickListStatus=true
                        }
                    },
                    Labels = new List<Label>()
                    {
                        new Label()
                        {
                            LabelData ="labeldata 1"
                        }
                    }
                }
            };
            todocontext.Notes.AddRange(notes);
            todocontext.SaveChanges();

        }
        [Fact]
        public async void Test1()
        {
            var res = await _controller.GetNotes();
            var OkObj = res as List<Notes>;
            Assert.Equal(1, OkObj.Count);
        }
    }
}
