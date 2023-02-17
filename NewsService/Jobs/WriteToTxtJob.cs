using NewsService.Services;
using Quartz;

namespace NewsService.Jobs
{
    public class WriteToTxtJob : IJob
    {
      
        public Task Execute(IJobExecutionContext context)
        {
            File.WriteAllText("./test.txt",DateTime.Now.ToLongTimeString());
            return Task.FromResult(true);
        }
    }
}
