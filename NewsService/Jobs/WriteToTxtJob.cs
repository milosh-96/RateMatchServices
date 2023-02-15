using NewsService.Services;
using Quartz;

namespace NewsService.Jobs
{
    public class WriteToTxtJob : IJob
    {
        private readonly IRssReadingService _readingService;

        public WriteToTxtJob(IRssReadingService readingService)
        {
            _readingService = readingService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            System.IO.File.WriteAllLines("test.txt", _readingService.GetAll().Select(x=>x.Title.Text).ToList());
            return Task.FromResult(true);
        }
    }
}
