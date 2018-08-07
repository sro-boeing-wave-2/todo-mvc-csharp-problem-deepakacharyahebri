using System;
using ToDoAssignment.Controllers;
using ToDoAssignment.Models;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

namespace ToDoAssignment.Tests
{
    public class UnitTestToDo
    {
        private NotesController _controller;
        private ToDoContext todocontext;
        public UnitTestToDo()
        {
            var dbOptionBuilder = new DbContextOptionsBuilder<ToDoContext>();
            dbOptionBuilder.EnableSensitiveDataLogging();
            dbOptionBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            todocontext = new ToDoContext(dbOptionBuilder.Options);
            _controller = new NotesController(todocontext);
            PrepareData(todocontext);
        }
        public void PrepareData(ToDoContext todocontext)
        {
            var notes =
                new Notes()
                {
                    Id = 2,
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
        public async void TestForGetAll()
        {
            //PrepareData(todocontext);
            var Res = await _controller.GetNotes();
            var ListOfNotes = Res as List<Notes>;
            Assert.Single(ListOfNotes);
            var ToBeChecked = ListOfNotes[0];
            //Console.WriteLine("Id ="+ToBeChecked.Id);
            Assert.Equal("My First Note", ToBeChecked.Title);
            Assert.Equal("This is my plaintext", ToBeChecked.PlainText);
            Assert.Equal(true, ToBeChecked.PinStatus);
        }

        [Fact]
        public async void TestGetById()
        {
            //PrepareData(todocontext);
            var r = await _controller.GetNotes();
            var ListOfNotes = r as List<Notes>;
            var ToBeChecked = ListOfNotes[0];
            //Console.WriteLine("Changed Id",ToBeChecked.Id);
            var result = await _controller.GetNote(ToBeChecked.Id);
            var OkObject = result as OkObjectResult;
            Console.WriteLine(OkObject.StatusCode);
            //Notes note = new Notes();
            var note = OkObject.Value as Notes;
            
            //Console.WriteLine("Fine");
            //Console.WriteLine(note.PlainText);
            //Assert.Equal(note.Title, "My First Note");
            note.PlainText.Should().Be("This is my plaintext");
            note.Title.Should().Be("My First Note");
            Assert.Equal(true, ToBeChecked.PinStatus);
        }

        [Fact]
        public async void SearchByTitle()
        {
            //PrepareData(todocontext);
            var result = await _controller.SearchByTitle("My First Note");
            var Lists = result as List<Notes>;
            var ToBeTested = Lists[0];
            Assert.Equal(1, Lists.Count);
            ToBeTested.PlainText.Should().Be("This is my plaintext");
            ToBeTested.PinStatus.Should().Be(true);
            ToBeTested.Title.Should().Be("My First Note");
        }

        [Fact]
        public async void SearchByLabel()
        {
            //PrepareData(todocontext);
            var result = await _controller.SearchByLabel("labeldata 1");
            var OkObj = result as OkObjectResult;
            var Notes = OkObj.Value as List<Notes>;
            Assert.Equal(OkObj.StatusCode, 200);
            var ToBeTested = Notes[0];
            ToBeTested.PlainText.Should().Be("This is my plaintext");
            ToBeTested.PinStatus.Should().Be(true);
            ToBeTested.Title.Should().Be("My First Note");
        }

        [Fact]
        public async void CheckingPinnedNotes()
        {
            var result = await _controller.PinnedNotes();
            var OkObj = result as OkObjectResult;
            var Notes = OkObj.Value as List<Notes>;
            Assert.Equal(OkObj.StatusCode, 200);
            var ToBeTested = Notes[0];
            ToBeTested.PlainText.Should().Be("This is my plaintext");
            ToBeTested.Title.Should().Be("My First Note");
            ToBeTested.PinStatus.Should().Be(true);
        }

        [Fact]
        public async void TestingPost()
        {
            Notes note = new Notes
            {
                Id = 5,
                Title = "POSTING",
                PlainText = "POSTING PLAIN TEXT",
                PinStatus = true,
            };
            var result = await _controller.PostNote(note);
            var resultAsOkObjectResult = result as CreatedAtActionResult;
            var ToBeTested = resultAsOkObjectResult.Value as Notes;
            ToBeTested.PlainText.Should().Be("POSTING PLAIN TEXT");
            ToBeTested.PinStatus.Should().Be(true);
            ToBeTested.Title.Should().Be("POSTING");
        }

        //[Fact]
        //public async void TestingPut()
        //{
        //    Notes note = new Notes
        //    {
        //        Id =2,
        //        Title = "PUT",
        //        PlainText = "Put sentence",
        //        PinStatus = true,
        //    };
        //    //var r = await _controller.GetNotes();
        //    //var ListOfNotes = r as List<Notes>;
        //    //var ToBeChecked = ListOfNotes[0];
        //    //note.Id = ToBeChecked.Id;
        //    var result = await _controller.PutNote(2, note);
        //    var resultAsOkObjectResult = result as OkObjectResult;
        //    //var notes = resultAsOkObjectResult.Value as Notes;
        //    Assert.Equal(resultAsOkObjectResult.StatusCode, 400);
        //}

        

        //[Fact]
        //public async void Test8()
        //{
        //    var result = await _controller.DeleteNote(7);
        //    var resultAsOkObjectResult = result as OkObjectResult;
        //    Console.WriteLine(resultAsOkObjectResult);
        //    var notes = resultAsOkObjectResult.Value as Notes;
        //    Assert.NotNull(notes);
        //}

        [Fact]
        public async void DeleteNodeByLabel()
        {
            var result = await _controller.DeleteNoteByLabel("labeldata 1");
            result.Should().BeOfType<OkResult>();
        }
    }
}
