using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WorkshopAPI.Data;
using WorkshopAPI.Mapper;
using WorkshopAPI.Repository;
using WorkshopAPI.Repository.IRepository;

namespace WorkshopAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(Options =>
            Options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddAutoMapper(typeof(Mappers));

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();


            //Swagger Docs Configuration
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("workshop-movies-api", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "workshop-movies-api",
                    Version = "1",
                    Description = "Movies Backend Application",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "rudy.unzueta@truextend.com",
                        Name = "R. Fernando Unzueta",
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                    }
                });

                //setting comments to the doc api
                var commentsfile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var commentsroute = Path.Combine(AppContext.BaseDirectory, commentsfile);
                options.IncludeXmlComments(commentsroute);
            });


            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            //SwaggerDocs
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/workshop-movies-api/swagger.json","Workshop");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
