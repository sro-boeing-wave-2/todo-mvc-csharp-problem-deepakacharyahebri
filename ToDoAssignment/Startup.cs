using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using ToDoAssignment.Models;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ToDoAssignment
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }
        
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            if (Environment.IsEnvironment("Testing"))
            {
                services.AddDbContext<ToDoContext>(options =>
                    options.UseInMemoryDatabase("TestingDB"));
            }
            else
            {
                services.AddDbContext<ToDoContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DockerToDoContext")));
            }


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //app.Use((context, next) => {
            //    Console.WriteLine("I am the Logger");
            //    return next();
            //});


            //app.Use((context, next) => {
            //    if (context.Request.Method == "GET" && context.Request.Path == "/ping")
            //    {
            //        return context.Response.WriteAsync("Pong");
            //    }
            //    else 
            //    {
            //        return next();
            //    }
            //});
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            /*var context = app.ApplicationServices.GetService<ToDoContext>(); //needed for docker
            context.Database.Migrate(); //needed for docker */


            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
