using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ShoppingListApi.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingListApi
{
    public class Startup
    {
        // Constructor that takes an IConfiguration object and sets the Configuration property.
        // This is injected by the ASP.NET Core runtime and provides access to configuration
        // settings such as those found in appsettings.json.
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // A property to hold the injected configuration object.
        public IConfiguration Configuration { get; }

        // This method is called by the runtime to add services to the DI container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configuring Cross-Origin Resource Sharing (CORS) to allow requests from the specified origin.
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            // Adds services required for using controllers.
            services.AddControllers();

            // Adds Swagger generation services.
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ShoppingListApi", Version = "v1" });
            });

            // Add Entity Framework context and configure it to use SQL Server with a connection string from configuration.
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
        }

        // This method is called by the runtime to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Enable the developer exception page in development environment.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Enable serving of Swagger JSON and Swagger UI in development environment.
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShoppingListApi v1"));
            }

            // Enable CORS with the default policy.
            app.UseCors();

            // Redirect HTTP requests to HTTPS.
            app.UseHttpsRedirection();

            // Enable routing.
            app.UseRouting();

            // Enable authorization middleware.
            app.UseAuthorization();

            // Enable endpoints for the app, such as MVC controller endpoints.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
