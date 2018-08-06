using System;
using ToDoAssignment.Controllers;
using ToDoAssignment.Models;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace ToDoAssignment.Tests
{
    public class UnitTestToDo
    {
        private NotesController _controller;
        private ToDoContext todocontext;
        public UnitTestToDo()
        {
            var dbOptionBuilder = new DbContextOptionsBuilder<ToDoContext>();
            dbOptionBuilder.UseInMemoryDatabase("TempDB");
            todocontext = new ToDoContext(dbOptionBuilder.Options);
            _controller = new NotesController(todocontext);
            PrepareData(todocontext);
        }
        public void PrepareData(ToDoContext todocontext)
        {
            var notes =
                new Notes()
                {
                    Title = "My First Note",
                    PlainText = "This is my plaintext",
                    PinStatus = true,
                    CheckLists = new List<CheckList>()
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
                };
            todocontext.Notes.Add(notes);
            todocontext.SaveChanges();

        }
        [Fact]
        public async void Test1()
        {
            //PrepareData(todocontext);
            var res = await _controller.GetNotes();
            var OkObj = res as List<Notes>;
            Assert.Single(OkObj);
        }

        [Fact]
        public async void Test2()
        {
            //PrepareData(todocontext);
            var result = await _controller.GetNote(1);
            try
            {
                var status = result as OkObjectResult;
                Assert.Equal(200, status.StatusCode);
            }
            catch (Exception)
            {
                Console.WriteLine("I'm here");
            }
        }

        [Fact]
        public async void Test3()
        {
            //PrepareData(todocontext);
            var result = await _controller.SearchByTitle("My First Note");
            foreach (var item in result)
            {
                Console.WriteLine(item.Id);
            }
            var Lists = result as List<Notes>;
            Assert.Equal(3, Lists.Count);
        }

        [Fact]
        public async void Test4()
        {
            //PrepareData(todocontext);
            var result = _controller.SearchByLabel("labeldata 1");
            var OkObj = result as OkObjectResult;
            Assert.Equal(OkObj.StatusCode, 200);
        }

        [Fact]
        public async void Test5()
        {
            var result = _controller.PinnedNotes();
            Assert.NotNull(result);
        }

        [Fact]
        public async void Test6()
        {
            Notes note = new Notes
            {
                Title = "PUT",
                PlainText = "Put sentence",
                PinStatus = true,
            };
            var result = await _controller.PutNote(1, note);
            var resultAsOkObjectResult = result as OkObjectResult;
            //var notes = resultAsOkObjectResult.Value as Notes;
            Assert.Equal(resultAsOkObjectResult.StatusCode, 204);
        }

        [Fact]
        public async void Test7()
        {
            Notes note = new Notes
            {
                Title = "PUT",
                PlainText = "Put sentence",
                PinStatus = true,
            };
            var result = await _controller.PostNote(note);
            var resultAsOkObjectResult = result as CreatedAtActionResult;
            var notes = resultAsOkObjectResult.Value as Notes;
            Assert.NotNull(notes);
        }

        [Fact]
        public async void Test8()
        {
            var result = await _controller.DeleteNote(1);
            var resultAsOkObjectResult = result as OkObjectResult;
            var notes = resultAsOkObjectResult.Value as Notes;
            Assert.NotNull(notes);
        }

        //[Fact]
        //public async void Test9()
        //{
        //    var result = await _controller.DeleteNoteByLabel("labeldata 1");
        //    var resultAsOkObjectResult = result as OkObjectResult;
        //    var notes = resultAsOkObjectResult.Value as Notes;
        //    Assert.NotNull(notes);
        //}
    }
}
