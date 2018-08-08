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
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace ToDoAssignment.Tests
{
    public class UnitTestToDo
    {
        public NotesController GetController()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ToDoContext>();
            optionsBuilder.UseInMemoryDatabase<ToDoContext>(Guid.NewGuid().ToString());
            var todocontext = new ToDoContext(optionsBuilder.Options);
            CreateTestData(optionsBuilder.Options);
            return new NotesController(todocontext);
        }

        public void CreateTestData(DbContextOptions<ToDoContext> options)
        {
            using (var todocontext = new ToDoContext(options))
            {
                var NotesToAdd = new List<Notes>
                {
                    new Notes()
                    {
                        Id = 1,
                        PlainText = "This is my plaintext",
                        PinStatus = true,
                        Title = "My First Note",
                        Labels = new List<Label>
                        {
                            new Label {
                                LabelData ="Label Data 1"
                            },
                            new Label {
                                LabelData ="Label Data 2"
                            }
                        },
                        CheckLists =new List<CheckList>
                        {
                            new CheckList {
                                CheckListData ="CheckList Data 1",
                                ChickListStatus =true
                            },
                            new CheckList {
                                CheckListData ="CheckList Data 2",
                                ChickListStatus =false
                            }
                        }
                    },
                    new Notes()
                    {
                        Id = 2,
                        PlainText = "PlainText 2",
                        PinStatus = false,
                        Title = "Title 2",
                        Labels = new List<Label>
                        {
                            new Label {
                                LabelData ="Label Data 3"
                            },
                            new Label {
                                LabelData ="Label Data 4"
                            }
                        },
                        CheckLists =new List<CheckList>
                        {
                            new CheckList {
                                CheckListData ="CheckList Data 3",
                                ChickListStatus =true
                            },
                            new CheckList {
                                CheckListData ="CheckList Data 4",
                                ChickListStatus =false
                            }
                        }
                    },
                };
                todocontext.Notes.AddRange(NotesToAdd);
                todocontext.SaveChanges();
            }
        }
        
        [Fact]
        public async void TestForGetAll()
        {
            //PrepareData(todocontext);
            var _controller = GetController();
            var Res = await _controller.GetNotes();
            var ListOfNotes = Res as List<Notes>;
            Assert.Equal(2, ListOfNotes.Count);
            var ToBeChecked = ListOfNotes[0];
            //Console.WriteLine("Id ="+ToBeChecked.Id);
            Assert.Equal("My First Note", ToBeChecked.Title);
            Assert.Equal("This is my plaintext", ToBeChecked.PlainText);
            Assert.Equal(true, ToBeChecked.PinStatus);
        }

        [Fact]
        public async void TestGetById()
        {
            var _controller = GetController();
            //PrepareData(todocontext);
            //var r = await _controller.GetNotes();
            //var ListOfNotes = r as List<Notes>;
            //var ToBeChecked = ListOfNotes[0];
            //Console.WriteLine("Changed Id",ToBeChecked.Id);
            var result = await _controller.GetNote(1);
            var OkObject = result as OkObjectResult;
            Console.WriteLine(OkObject.StatusCode);
            //Notes note = new Notes();
            var note = OkObject.Value as Notes;
            
            //Console.WriteLine("Fine");
            //Console.WriteLine(note.PlainText);
            //Assert.Equal(note.Title, "My First Note");
            note.PlainText.Should().Be("This is my plaintext");
            note.Title.Should().Be("My First Note");
            note.PinStatus.Should().Be(true);
        }

        [Fact]
        public async void SearchByTitle()
        {
            var _controller = GetController();
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
            var _controller = GetController();
            //PrepareData(todocontext);
            var result = await _controller.SearchByLabel("Label Data 1");
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
            var _controller = GetController();
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
            var _controller = GetController();
            Notes note = new Notes
            {
                Id = 3,
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

        [Fact]
        public async void TestingPut()
        {
            var _controller = GetController();
            //var r = await _controller.GetNotes();
            //var ListOfNotes = r as List<Notes>;
            //var ToBeChecked = ListOfNotes[0];
            //note.Id = ToBeChecked.Id;
            var NotePut = new Notes()
            {
                Id = 2,
                Title = "Updated"
            };
            //var json = JsonConvert.SerializeObject(NotePut);
            //var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var result = await _controller.PutNote(2, NotePut);
            var resultAsOkObjectResult = result as NoContentResult;
            //var notes = resultAsOkObjectResult.Value as Notes;
            Assert.Equal(resultAsOkObjectResult.StatusCode, 204);
        }
        



        [Fact]
        public async void DeleteById()
        {
            var _controller = GetController();
            var result = await _controller.DeleteNote(1);
            var resultAsOkObjectResult = result as OkObjectResult;
            Assert.Equal(resultAsOkObjectResult.StatusCode, 200);
        }

        [Fact]
        public async void DeleteNodeByLabel()
        {
            var _controller = GetController();
            var result = await _controller.DeleteNoteByLabel("Label Data 4");
            result.Should().BeOfType<OkResult>();
        }
    }
}
