using System.ServiceModel.Syndication;

namespace NewsService.Services
{
    public interface IRssReadingService
    {
        public IEnumerable<SyndicationItem> GetAll();
        public IEnumerable<SyndicationItem> GetAll(int limit);
        public IEnumerable<SyndicationItem> GetAll(int limit, int offset);
    }
}
