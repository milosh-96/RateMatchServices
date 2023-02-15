using Microsoft.EntityFrameworkCore;
using NewsService.Data;
using NewsService.Jobs;
using NewsService.Services;
using Quartz;

namespace NewsService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration["ConnectionStrings:Default"]);
            }); 
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();
                q.AddJob<FetchMarcaEnRssFeedJob>(opts => opts.WithIdentity("FetchMarcaEn"));
                q.AddJob<FetchEspnFootballRssFeed>(opts => opts.WithIdentity("FetchEspn"));

                q.AddTrigger(opts => opts
                    .ForJob("FetchMarcaEn")
                    .WithIdentity("FetchMarcaEn-trigger")
                    .WithSimpleSchedule(
                        x => x.WithIntervalInMinutes(30)
                    )
                    ); 
                q.AddTrigger(opts => opts
                    .ForJob("FetchEspn")
                    .WithIdentity("FetchEspn-trigger")
                    .WithSimpleSchedule(
                        x => x.WithIntervalInMinutes(30)
                    ));
            });

            // ASP.NET Core hosting
            builder.Services.AddQuartzServer(options =>
            {
                // when shutting down we want jobs to complete gracefully
                options.WaitForJobsToComplete = true;
            });

            //Custom Services //

            builder.Services.AddScoped<RssReadingService>();
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}