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

                foreach (var item in NewsSources.Get())
                {
                    JobDataMap dataMap = (new JobDataMap());
                    dataMap.Put("feed", item);

                    q.AddTrigger(opts => opts
                        .ForJob("FetchRssFeed")
                        .WithIdentity("Fetch"+item.Title.Text+"-trigger")
                        .UsingJobData(dataMap)
                        .WithSimpleSchedule(
                            x => x.WithInterval(TimeSpan.FromMinutes(30)).RepeatForever()
                        )
                        );
                }
               
              

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