using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PictureFinder.Application.WebServices;
using PictureFinder.Data.Repositories;
using PictureFinder.Data.Sql;
using PictureFinder.Data.Sql.Repository;
using PictureFinder.Integration.Telegram;
using PictureFinder.Presentation.ExceptionHandling;

namespace PictureFinder.Presentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Program));

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("Default"),
                    sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(
                            typeof(ApplicationDbContext).GetTypeInfo().Assembly.GetName().Name);
                    }));

            services.AddTransient<IPhotoRepository, PhotoRepository>();
            services.AddTransient<ITagRepository, TagRepository>();

            services.AddSingleton(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();

                return new TelegramBotConfiguration(
                    configuration["TelegramBot:BaseBotApiUrl"],
                    configuration["TelegramBot:BaseFilesApiUrl"],
                    configuration["TelegramBot:ApiKey"]);
            });

            services.AddTransient<ITelegramBotClient, TelegramBotClient>();
            services.AddSingleton(_ => new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            });

            services.AddTransient<ITelegramService, TelegramService>();

            services.AddLogging();

            services.AddControllers().AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                };
            });

            
            services.AddHealthChecks();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Telegram}/{action=Update}/{id?}");
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}