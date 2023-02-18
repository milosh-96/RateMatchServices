using Microsoft.EntityFrameworkCore;
using NewsService.Data;
using NewsService.Services;
using Quartz;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.Syndication;

namespace NewsService.Jobs
{
    public class FetchRssFeedJob : IJob
    {
        private RssReadingService rssReadingService;
        private IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public FetchRssFeedJob(RssReadingService rssReadingService, IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            this.rssReadingService = rssReadingService;
            _dbContextFactory = dbContextFactory;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var dataMap = context.MergedJobDataMap;
            SyndicationFeed feed = (SyndicationFeed)dataMap["feed"];
            using var dbContext = _dbContextFactory.CreateDbContext();
            rssReadingService.SetRssFeed(feed);
            var items = rssReadingService.GetAll();
            var toAdd = new List<ExternalContentLink>();
            items.ToList().ForEach(x =>
            {
                try
                {
                    var link = "https://r8match.com";
                    
                        link = x.Links.Where(x=>x.MediaType==null).FirstOrDefault().Uri.ToString();
                    if (!dbContext.ExternalContentLinks.Any(y => y.Title == x.Title.Text.ToString()
                     && y.ExternalUrl == link))
                    {
                        toAdd.Add(
                       new ExternalContentLink()
                       {
                           Source = feed.Title.Text,
                           Title = x.Title.Text,
                           CreatedAt = x.PublishDate.DateTime.ToUniversalTime(),
                           ExternalUrl = link
                       }
                       );
                    }
                }
                catch (Exception e)
                {

                }
            });
            if (toAdd.Count > 0)
            {
                dbContext.ExternalContentLinks.AddRange(toAdd);
                dbContext.SaveChanges();
            }
            File.AppendAllText("./test.txt", 
                "Fetched " 
                + feed.Title.Text 
                + "@ " 
                +  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                + "and inserted " + toAdd.Count + " items."
                + Environment.NewLine

                );

            return Task.FromResult(true);
        }
    }
}
