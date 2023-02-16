using Microsoft.EntityFrameworkCore;
using NewsService.Data;
using NewsService.Jobs;
using NewsService.Services;
using Quartz;
using System.ServiceModel.Syndication;

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
                q.AddJob<FetchRssFeedJob>(opts => opts.WithIdentity("FetchRssFeed"));

                JobDataMap espnDataMap = (new JobDataMap());
                espnDataMap.Put("feed",new SyndicationFeed()
                {
                    Title = new TextSyndicationContent("ESPN"),
                    BaseUri = new Uri("https://www.espn.com/espn/rss/soccer/news")
                }); 
                
                JobDataMap dailyMailDataMap = (new JobDataMap());
                dailyMailDataMap.Put("feed",new SyndicationFeed()
                {
                    Title = new TextSyndicationContent("Daily Mail Sports"),
                    BaseUri = new Uri("https://www.dailymail.co.uk/sport/index.rss")
                }); 
                
                q.AddTrigger(opts => opts
                    .ForJob("FetchRssFeed")
                    .WithIdentity("FetchEspn-trigger")
                    .UsingJobData(espnDataMap)
                    .WithSimpleSchedule(
                        x => x.WithIntervalInMinutes(30)
                    )
                    ); 
                
                q.AddTrigger(opts => opts
                    .ForJob("FetchRssFeed")
                    .WithIdentity("FetchDailyMail-trigger")
                    .UsingJobData(dailyMailDataMap)
                    .WithSimpleSchedule(
                        x => x.WithIntervalInMinutes(30)
                    )
                    ); 
               
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