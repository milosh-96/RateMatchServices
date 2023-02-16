using System.ServiceModel.Syndication;
using System.Xml;

namespace NewsService.Services
{
    public class RssReadingService : IRssReadingService
    {
        private SyndicationFeed? rssFeed;

        public void SetRssFeed(SyndicationFeed feed)
        {
            rssFeed = feed; 
        }
        public IEnumerable<SyndicationItem> GetAll()
        {
            if (this.rssFeed != null)
            {
                using var reader = XmlReader.Create(this.rssFeed.BaseUri.ToString());
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
