using Infrastructure.Cache;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ContactApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebServerController : Controller
    {
        private readonly ICacheHelper<int> _cacheHelper;
        private readonly ILogger<WebServerController> _logger;

        public WebServerController(ICacheHelper<int> cacheHelper, ILogger<WebServerController> logger)
        {
            _cacheHelper = cacheHelper;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Index(User user)
        {
            try
            {
                _logger.LogInformation(String.Format("Creating user {0}", user.Number));
                var item = _cacheHelper.GetOrCreate(user.Number, () => { return user.Number % 1234; });
                return Ok(item);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
           

       
        }
    }
}
