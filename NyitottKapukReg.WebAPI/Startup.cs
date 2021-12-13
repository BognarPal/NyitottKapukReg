using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NyitottKapukReg.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NyitottKapukRegWebAPI
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
            services.AddControllers();
            services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseMySql(Configuration.GetConnectionString("nyitottkapukreg"));
            });
#if DEBUG
            services.AddCors(option =>
            {
                option.AddPolicy("EnableCORS", builder =>
                {
                    builder.SetIsOriginAllowed(origin => origin.StartsWith("http://localhost:4200"))
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials()
                           .Build();
                });
            });
#endif
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/v1/swagger.json", "Nyitott kapuk API V1");
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
#if DEBUG
            app.UseCors("EnableCORS");
#endif

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
