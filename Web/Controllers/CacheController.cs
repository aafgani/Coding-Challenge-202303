using Infrastructure.HttpClientService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model.AppConfig;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Web.Controllers
{
    public class CacheController : Controller
    {
        private readonly IWebCacheServiceClient serviceClient;
        private readonly WebCacheServiceConfiguration config;

        public CacheController(IWebCacheServiceClient serviceClient, IOptions<WebCacheServiceConfiguration> options)
        {
            this.serviceClient = serviceClient;
            this.config = options.Value;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var queryCache = await serviceClient.QueryCache();
            ViewBag.JsonData = Newtonsoft.Json.Linq.JValue.Parse(JsonConvert.SerializeObject(queryCache)).ToString(Newtonsoft.Json.Formatting.Indented);
            ViewBag.BaseURL = config.BaseURL;
            return View();
        }
    }
}
