using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Converters;
using Nohwnd.Meteo.Core;
using Nohwnd.Meteo.Data.Mongo;

namespace Nohwnd.Meteo.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddCors(o => o.AddPolicy("p", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });

            var databaseSettings = new DatabaseSettings();
            _configuration.GetSection("Database").Bind(databaseSettings);


            // add database
            var db = new MongoClient(databaseSettings.ConnectionString).GetDatabase(databaseSettings.Name);
            var weatherCollection = db.GetCollection<Weather>(typeof(Weather).Name.ToLowerInvariant());

            // seed
            if (!weatherCollection.AsQueryable().Any())
            {
                weatherCollection.InsertMany(new[]
                {
                    new Weather
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        City = "Prague",
                        WeatherType = WeatherType.Rainy,
                    },
                    new Weather
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        City = "Basel",
                        WeatherType = WeatherType.Sunny,
                    },
                });
            }

            services.AddSingleton(weatherCollection);

            // add everythying from Data
            services.Scan(s => s.FromAssemblyOf<WeatherRepository>().AddClasses().AsImplementedInterfaces());

            // add everything from Core
            services.Scan(s => s.FromAssemblyOf<WeatherService>().AddClasses().AsImplementedInterfaces());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("p");
            app.UseMvc();
        }
    }
}