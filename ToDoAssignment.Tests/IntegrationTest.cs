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
                new Notes()
                {
                    Id = 1,
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
                new Notes()
                {
                    Id = 1,
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
            Assert.Equal(JObjectNotes["id"].ToString(), "1");
            Assert.Equal(JObjectNotes["title"].ToString(), "My First Note");
            Assert.Equal(JObjectNotes["plainText"].ToString(), "This is my plaintext");
            Assert.Equal(JObjectNotes["pinStatus"].ToString(), "True");

            var DeleteResponse = await _client.DeleteAsync("/api/notes/1");
            DeleteResponse.EnsureSuccessStatusCode();
        }
    }
}
