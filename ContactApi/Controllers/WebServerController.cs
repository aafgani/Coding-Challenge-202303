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

        public WebServerController(ICacheHelper<int> cacheHelper)
        {
            _cacheHelper = cacheHelper;
        }

        [HttpPost]
        public IActionResult Index(User user)
        {
            var item = _cacheHelper.GetOrCreate(user.Number, () => { return user.Number % 1234; });

            return Ok(item);
        }
    }
}
