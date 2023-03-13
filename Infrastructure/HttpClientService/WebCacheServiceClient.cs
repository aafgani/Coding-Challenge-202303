using ContactApi.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Model;
using Model.AppConfig;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.HttpClientService
{
    public class WebCacheServiceClient : IWebCacheServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WebCacheServiceClient> _logger;

        public WebCacheServiceClient(HttpClient httpClient, ILogger<WebCacheServiceClient> logger, IOptions<WebCacheServiceConfiguration> options)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(options.Value.BaseURL);
            _logger = logger;
        }

        private async Task<T> SendAsync<T>(Object param, string url, HttpMethod httpMethod)
        {
            try
            {
                var urls = _httpClient.BaseAddress + url;
                var body = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, "application/json");
                HttpRequestMessage request = new HttpRequestMessage(httpMethod, url) { Content = body };
                HttpResponseMessage response = await _httpClient.SendAsync(request);
                var result = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<T>(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        public async Task<int> CreateUser(User user)
        {
            var url = HttpClientUrl.CreateUser;
            _logger.LogInformation(String.Format(" send to {0} : body {1}", url, JsonConvert.SerializeObject(user)));
            var result = await SendAsync<int>(user, url, HttpMethod.Post);
            return result;
        }

        public async Task<CacheStatus<int>> QueryCache()
        {
            var url = HttpClientUrl.GetCacheStats;
            _logger.LogInformation(String.Format(" send to {0} ", url));
            var result = await SendAsync<CacheStatus<int>>(null, url, HttpMethod.Get);
            return result;
        }
    }
}
