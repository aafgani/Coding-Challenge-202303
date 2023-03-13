using Infrastructure.Cache;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CacheController : Controller
    {
        private readonly ICacheHelper<int> _cacheHelper;

        public CacheController(ICacheHelper<int> cacheHelper)
        {
            _cacheHelper = cacheHelper;
        }

        [HttpPost]
        public IActionResult Index()
        {
            var statistic = _cacheHelper.GetStatus();
            return Ok(statistic);
        }
    }
}
