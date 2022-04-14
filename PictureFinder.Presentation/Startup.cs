using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PictureFinder.Application.WebServices;
using PictureFinder.Data.Repositories;
using PictureFinder.Data.Sql;
using PictureFinder.Data.Sql.Repository;
using PictureFinder.Integration.Telegram;
using PictureFinder.Presentation.ExceptionHandling;
using PictureFinder.Presentation.HostedService;
using Serilog;

namespace PictureFinder.Presentation
{
    public class Startup
    {
        public Startup(
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            if (Environment.IsDevelopment()) services.AddHostedService<TunnelService>();

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
            services.AddTransient<IPhotoService, PhotoService>();

            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new DefaultNamingStrategy()
                };
            });


            services.AddHealthChecks();


            ConfigureModelBindingExceptionHandling(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
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

            loggerFactory.AddSerilog();
            app.UseHttpsRedirection();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Photo}/{action=Index}/{id?}");
                endpoints.MapHealthChecks("/health");
            });
        }

        private static void ConfigureModelBindingExceptionHandling(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var error = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .Select(e => new ValidationProblemDetails(actionContext.ModelState)).FirstOrDefault();

                    Log.Error("{@RequestPath} received invalid message format: {@Exception}. {@ModelState}",
                        actionContext.HttpContext.Request.Path.Value,
                        error.Errors.Values,
                        actionContext.ModelState.Keys);
                    return new BadRequestObjectResult(error);
                };
            });
        }
    }
}