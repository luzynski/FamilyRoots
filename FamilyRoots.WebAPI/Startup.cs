using FamilyRoots.WebAPI.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Neo4j.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using GraphDatabase = FamilyRoots.WebAPI.Persistence.GraphDatabase;
using IApplicationLifetime = Microsoft.Extensions.Hosting.IApplicationLifetime;

namespace FamilyRoots.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Formatting = Formatting.Indented;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });
            services.AddSwaggerGen();
            services.AddSwaggerGenNewtonsoftSupport();
            services.AddRouting(options => options.LowercaseUrls = true);
            
            services.AddSingleton(Neo4j.Driver.GraphDatabase.Driver(
                "bolt://localhost:7687", 
                AuthTokens.Basic("neo4j", "neo4j")));
            services.AddSingleton<IGraphDatabase, GraphDatabase>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FamilyRoots API");
                c.RoutePrefix = string.Empty;
            });
            
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
