using System.ServiceModel.Syndication;

namespace NewsService.Data
{
    public static class NewsSources
    {
        public static List<SyndicationFeed> Get()
        {
            return new List<SyndicationFeed>() {
                new SyndicationFeed()
            {
                Title=new TextSyndicationContent("ESPN"),
                BaseUri=new Uri("https://www.espn.com/espn/rss/soccer/news")
            },
                new SyndicationFeed()
                {
                    Title=new TextSyndicationContent("Daily Mail Sports"),
                    BaseUri=new Uri("https://www.dailymail.co.uk/sport/index.rss")
                }, 
                new SyndicationFeed()
                {
                    Title=new TextSyndicationContent("Yahoo! Sports"),
                    BaseUri=new Uri("https://sports.yahoo.com/rss/")
                },
                new SyndicationFeed()
                {
                    Title=new TextSyndicationContent("Tribal Football"),
                    BaseUri=new Uri("https://www.tribalfootball.com/rss/mediafed/general/rss.xml")
                },
                new SyndicationFeed()
                {
                    Title=new TextSyndicationContent("FootballCritic"),
                    BaseUri=new Uri("https://www.footballcritic.com/rss/")
                },
                new SyndicationFeed()
                {
                    Title=new TextSyndicationContent("MLB.com"),
                    BaseUri=new Uri("https://www.mlb.com/feeds/news/rss.xml")
                },
                new SyndicationFeed()
                {
                    Title=new TextSyndicationContent("90min.com"),
                    BaseUri=new Uri("https://www.90min.com/posts.rss")
                },
              

            };
        }
    }
}
