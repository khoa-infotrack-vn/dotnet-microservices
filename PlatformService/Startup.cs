using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PlatformService.Data;
using PlatformService.Repositories;
using PlatformService.SyncDataServices.Http;

namespace PlatformService
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _env = env;
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine("--> Using InMem Db");
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMem");
            });
            //if (_env.IsProduction())
            //{
            //    Console.WriteLine("--> Using SqlServer Db");
            //    services.AddDbContext<AppDbContext>(options =>
            //    {
            //        options.UseSqlServer(Configuration["ConnectionStrings:PlatformsConn"]);
            //    });
            //}
            //else
            //{
            //    Console.WriteLine("--> Using InMem Db");
            //    services.AddDbContext<AppDbContext>(options =>
            //    {
            //        options.UseInMemoryDatabase("InMem");
            //    });
            //}

            services.AddScoped<IPlatformRepository, PlatformRepository>();
            services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlatformService", Version = "v1" });
            });

            Console.WriteLine($"--> CommandService Endpoint {Configuration["CommandService"]}");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlatformService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            PrepData.PrePopulation(app, env.IsProduction());
        }
    }
}
