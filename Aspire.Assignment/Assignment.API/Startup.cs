using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Assignment.Infrastructure;
using Assignment.Core;
using Microsoft.AspNetCore.Mvc;
using Assignment.Core.Security;
using Assignment.Contracts.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Assignment.Migrations;
using Microsoft.Extensions.Logging;
using Google;
using Microsoft.EntityFrameworkCore;
using Assignment.API;
using Assignment.Contracts.DTO;
using System;
using Assignment.Core.Exceptions;
using NLog;
using NLog.Extensions.Logging;

namespace Assignment
{
    public class Startup
    {
        
        public const string MyAllowSpecificOrigins = "MyAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors();
            services.AddPersistence(Configuration);
            services.AddCore();
            services.AddMarketplaceAuthentication(Configuration);
            services.Configure<AppSettingsDTO>(
            Configuration.GetSection("ApplicationSettings"));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddDbContext<DatabaseContext>(options => options.UseSqlite(Configuration.GetConnectionString("ConnectionStrings")));
            services.AddControllers();
            services.AddCors(opt =>
            {
                opt.AddPolicy(name: "CorsPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:4200", "https://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Aspire Assignment App API", Description = "Aspire Assignment App  is a  solution, built to demonstrate the trainees to create the application", Version = "v1" });
            });

            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var _logger = LogManager.Setup().LoadConfigurationFromSection(Configuration).GetCurrentClassLogger();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(options => options.WithOrigins("https://localhost:44491/").AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            //     .AllowAnyHeader()
            //.AllowAnyMethod()
            //.AllowAnyOrigin()

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseHttpsRedirection();


            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Aspire Assignment App API v1");
            });
            app.UseCors("CorsPolicy");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
