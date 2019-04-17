using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Nohwnd.Meteo.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
      
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // modify the vue.js discovery document based on env variable            
            var apiUrl = _configuration.GetSection("Services")["Api"];
            var discoveryPath = Path.Combine(env.WebRootPath, "discovery.json");
            File.WriteAllLines(discoveryPath, new[]
            {
                JsonConvert.SerializeObject(
                    new
                    {
                        api =
                            apiUrl
                    }, Formatting.Indented)
            });
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}