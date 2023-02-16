using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsService.Data;

namespace NewsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public NewsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("/")]
        public IActionResult Get()
        {
            return new JsonResult(_dbContext.Articles.OrderByDescending(x=>x.PublishedAt).ToList());
        }
    }
}
