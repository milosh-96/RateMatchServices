using System.ServiceModel.Syndication;
using System.Xml;

namespace NewsService.Services
{
    public class RssReadingService : IRssReadingService
    {
        private string? rssFeedUrl;

        public void SetRssFeed(string url)
        {
            rssFeedUrl = url; 
        }
        public IEnumerable<SyndicationItem> GetAll()
        {
            if (this.rssFeedUrl != null)
            {
                using var reader = XmlReader.Create(this.rssFeedUrl);
                var feed = SyndicationFeed.Load(reader);
                return feed.Items;
            }
            throw new InvalidOperationException("Please set RSS feed Url!");
        }

        public IEnumerable<SyndicationItem> GetAll(int limit)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SyndicationItem> GetAll(int limit, int offset)
        {
            throw new NotImplementedException();
        }
    }
}
