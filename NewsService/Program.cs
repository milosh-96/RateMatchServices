using Microsoft.EntityFrameworkCore;
using NewsService.Data;
using NewsService.Jobs;
using NewsService.Services;
using Quartz;
using System.ServiceModel.Syndication;
using static Quartz.Logging.OperationName;

namespace NewsService
{
    public class Program
    {
        public static async Task Main(string[] args)
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
                //q.AddJob<WriteToTxtJob>(opts => opts.WithIdentity("WriteTxt","Jobs"));

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
                JobDataMap mlbDataMap = (new JobDataMap());
                mlbDataMap.Put("feed",new SyndicationFeed()
                {
                    Title = new TextSyndicationContent("MLB.com"),
                    BaseUri = new Uri("https://www.mlb.com/feeds/news/rss.xml")
                }); 
                
                q.AddTrigger(opts => opts
                    .ForJob("FetchRssFeed")
                    .WithIdentity("FetchEspn-trigger")
                    .UsingJobData(espnDataMap)
                    .WithSimpleSchedule(
                        x => x.WithInterval(TimeSpan.FromMinutes(30)).RepeatForever()
                    )
                    ); 
                
                q.AddTrigger(opts => opts
                    .ForJob("FetchRssFeed")
                    .WithIdentity("FetchDailyMail-trigger")
                    .UsingJobData(dailyMailDataMap)
                    .WithSimpleSchedule(
                        x => x.WithInterval(TimeSpan.FromMinutes(30)).RepeatForever()
                    )
                    );
                q.AddTrigger(opts => opts
                    .ForJob("FetchRssFeed")
                    .WithIdentity("MlbCom-trigger")
                    .UsingJobData(mlbDataMap)
                    .WithSimpleSchedule(
                        x => x.WithInterval(TimeSpan.FromMinutes(30))
                        .RepeatForever()
                    )
                    );

                //q.AddTrigger(opts => opts.WithIdentity("WriteTxtTrigger", "JobTriggers")
                //.ForJob("WriteTxt", "Jobs")
                //.WithSimpleSchedule(x =>
                //{
                //    x.WithInterval(TimeSpan.FromSeconds(10)).RepeatForever();
                //}
                //)
                //);

            });


          
            builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            // ASP.NET Core hosting
         



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