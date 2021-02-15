using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RouletteAPP.BLL;
using RouletteAPP.BLL.Abstract;
using RouletteAPP.Cache;
using RouletteAPP.Cache.Abstract;
using RouletteAPP.Data.Helpers;
using RouletteAPP.Data.Service.Abstract;
using RouletteAPP.Service.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteAPP.Api
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
            services.AddControllers();
            services.AddScoped<IRouletteManager, RouletteManager>();
            services.AddScoped<IRouletteService, RouletteService>();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddScoped<ICacheService, RedisCacheService>();
            SqlHelper.ConnectionString = Configuration.GetConnectionString("rouletteApp");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
