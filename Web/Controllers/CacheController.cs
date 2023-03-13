using Infrastructure.HttpClientService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Web.Controllers
{
    public class CacheController : Controller
    {
        private readonly IWebCacheServiceClient serviceClient;

        public CacheController(IWebCacheServiceClient serviceClient)
        {
            this.serviceClient = serviceClient;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var queryCache = await serviceClient.QueryCache();
            ViewBag.JsonData = JsonConvert.SerializeObject(queryCache);
            return View();
        }
    }
}
