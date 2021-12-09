using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using Dx29.Services;

namespace Dx29.BioEntity
{
    public class Startup
    {
        public const string VERSION = "v0.13.0";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dx29 BioEntity", Version = VERSION });
            });

            services.AddSingleton<BioEntityServiceEN>();
            services.AddSingleton<BioEntityServiceES>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, BioEntityServiceEN bioEntityServiceEN, BioEntityServiceES bioEntityServiceES)
        {
            bioEntityServiceEN.Initialize();
            bioEntityServiceES.Initialize();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"Dx29 BioEntity {VERSION}"));

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
