﻿using Microsoft.EntityFrameworkCore;
using NewsService.Data;
using NewsService.Services;
using Quartz;

namespace NewsService.Jobs
{
    public class FetchEspnFootballRssFeed : IJob
    {
        private RssReadingService rssReadingService;
        private IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public FetchEspnFootballRssFeed(RssReadingService rssReadingService, IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            this.rssReadingService = rssReadingService;
            _dbContextFactory = dbContextFactory;
        }

        public Task Execute(IJobExecutionContext context)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            rssReadingService.SetRssFeed("https://www.espn.com/espn/rss/soccer/news");
            var items = rssReadingService.GetAll();
            items.ToList().ForEach(x =>
            {
                try
                {
                    dbContext.Articles.Add(
                   new NewsArticle()
                   {
                       Source = "ESPN",
                       Title = x.Title.Text,
                       PublishedAt = x.PublishDate.DateTime.ToUniversalTime()
                   }
                   );
                    dbContext.SaveChanges();
                }
                catch (Exception e)
                {

                }
            });
            return Task.FromResult(true);
        }
    }
}
