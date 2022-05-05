using Defence22.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;


namespace Defence22
{
    public class Startup
    {

        //startup.cs inneholder en del extensions og funksjoner som tillater og bygger diverse videre 

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        
        //f.eks, denne metoden legger til servicer i containeren.
        //Her har vi prøvd oss på servicer som kjører programmene vi har laget
        //service.AddCors skal foreksempel tillate å kjøre kall mellom frontend og backend
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(
                options =>
                {
                    options.AddPolicy("AllowAll",
                        services => services
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        );
                }
                );
            services.AddResponseCaching();

            services.AddTransient<Services.Soldier>();

            services.AddTransient<Services.Mission>();

            services.AddTransient<Services.Vehicle>();

            services.AddTransient<Services.Weapon>();

            services.AddControllers();

            services.AddSwaggerGen();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddEntityFrameworkSqlite().AddDbContext<ApplicationDbContext>();

            using (var client = new ApplicationDbContext())
            {
                client.Database.EnsureCreated();
                client.SaveChanges();
            }
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext applicationDbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }


            DefaultFilesOptions newOptions = new DefaultFilesOptions();
            newOptions.DefaultFileNames.Add("index.html");

            app.UseSwaggerUI();

            app.UseDefaultFiles(newOptions);

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            
            app.UseCors("AllowAll");


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
