using HomeAutomation.Server.Interfaces;
using HomeAutomation.Server.Services;
using HomeAutomation.Server.Services.Background;
using HomeAutomation.Server.Services.Development;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HomeAutomation.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            if (this.Env.IsDevelopment())
            {
                services.AddSingleton<IHeaterService, DevelopmentHeaterService>();
                services.AddSingleton<ILightingService, DevelopmentLightingService>();
                services.AddSingleton<ITemperatureService, DevelopmentTemperatureService>();
            }
            else
            {
                services.AddSingleton<IHeaterService, HeaterService>();
                services.AddSingleton<ILightingService, LightingService>();
                services.AddSingleton<ITemperatureService, TemperatureService>();
            }

            services.AddSingleton<ISolarEventsService, SolarEventsService>();
            services.AddHostedService<LightingControlService>();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
