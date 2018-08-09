using System;
using ToDoAssignment.Controllers;
using ToDoAssignment.Models;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using FluentAssertions;

namespace ToDoAssignment.Tests
{
    public class IntegrationTest
    {
        private HttpClient _client;
        public IntegrationTest()
        {
            var host = new TestServer(new WebHostBuilder()
                .UseEnvironment("Testing")
                .UseStartup<Startup>());
            _client = host.CreateClient();
        }

        [Fact]
        public async Task TestGetRequestAsync()
        {
            var Response = await _client.GetAsync("/api/notes");
            var ResponseBody = await Response.Content.ReadAsStringAsync();
            //Console.WriteLine("Body"+ResponseBody.);
            Response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task TestGetById()
        {
            var Response = await _client.GetAsync("/api/notes/99");
            //Console.WriteLine(await Response.Content.ReadAsStringAsync());
            //var ResponseBody = await Response.Content.ReadAsStringAsync();
            Assert.Equal(Response.StatusCode.ToString(), "NotFound");
        }

        [Fact]
        public async Task TestPost()
        {
            var notes =
                new Note()
                {
                    Id = 1,
                    Title = "My First Note",
                    PlainText = "This is my plaintext",
                    PinStatus = true,
                    CheckList = new List<CheckList>()
                    {
                        new CheckList()
                        {
                            CheckListData="checklist data 1",
                            CheckListStatus=true
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
            var json = JsonConvert.SerializeObject(notes);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var Response = await _client.PostAsync("/api/notes", stringContent);
            var ResponseGet = await _client.GetAsync("/api/notes");
            Console.WriteLine(await ResponseGet.Content.ReadAsStringAsync());
            ResponseGet.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task TestGetByIdAfterPost()
        {
            var notes =
                new Note()
                {
                    Id = 1,
                    Title = "My First Note",
                    PlainText = "This is my plaintext",
                    PinStatus = true,
                    CheckList = new List<CheckList>()
                    {
                        new CheckList()
                        {
                            CheckListData="checklist data 1",
                            CheckListStatus=true
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
            var json = JsonConvert.SerializeObject(notes);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var Response = await _client.PostAsync("/api/notes", stringContent);

            var ResponseForGetOne = await _client.GetAsync("/api/notes/1");
            ResponseForGetOne.EnsureSuccessStatusCode();

            var ResponseForGetFirst = await _client.GetAsync("/api/notes/search/First");
            var result = await ResponseForGetFirst.Content.ReadAsStringAsync();
            Console.WriteLine(await ResponseForGetFirst.Content.ReadAsStringAsync());
            var JArrayFormat = JArray.Parse(result);
            var JObjectNotes = JArrayFormat[0];
            Assert.Equal("1", JObjectNotes["id"].ToString());
            Assert.Equal("My First Note", JObjectNotes["title"].ToString());
            Assert.Equal("This is my plaintext", JObjectNotes["plainText"].ToString());
            Assert.Equal("True", JObjectNotes["pinStatus"].ToString());

            var DeleteResponse = await _client.DeleteAsync("/api/notes/1");
            DeleteResponse.EnsureSuccessStatusCode();

            var ResponsePost = await _client.PostAsync("/api/notes", stringContent);

            Note note = new Note
            {
                Id = 1,
                Title = "Suits",
                PlainText = "Season 6 is Releasing Soon",
                PinStatus = false,
            };
            var dataPut = JsonConvert.SerializeObject(note);
            var stringContentPut = new StringContent(dataPut, UnicodeEncoding.UTF8, "application/json");
            var TestPut = await _client.PutAsync("/api/Notes/1", stringContentPut);
            Assert.Equal(TestPut.StatusCode.ToString(), "NoContent");
            var TestGetByIdAfterPut = await _client.GetAsync("/api/Notes/1");
            var contentAfterPut = await TestGetByIdAfterPut.Content.ReadAsStringAsync();
            Console.WriteLine(contentAfterPut);
            //var ActualDataToTestGetAfterPut = JObject.Parse(contentAfterPut);
            ////var ActualDataToTestGet = ActualData[0];
            //Assert.Equal(ActualDataToTestGetAfterPut["Id"].ToString(), "1");
            //Assert.Equal(ActualDataToTestGetAfterPut["Title"], "Young Sheldon");

        }
    }
}
