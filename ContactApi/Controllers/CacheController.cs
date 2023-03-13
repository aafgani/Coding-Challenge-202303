using Infrastructure.Cache;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CacheController : Controller
    {
        private readonly ICacheHelper<int> _cacheHelper;
        private readonly ILogger<CacheController> _logger;

        public CacheController(ICacheHelper<int> cacheHelper, ILogger<CacheController> logger)
        {
            _cacheHelper = cacheHelper;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Index()
        {
            var statistic = _cacheHelper.GetStatus();
            return Ok(statistic);
        }
    }
}
