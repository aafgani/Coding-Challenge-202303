using ContactApi.Model;
using Model;

namespace Infrastructure.HttpClientService
{
    public interface IWebCacheServiceClient
    {
        Task<int> CreateUser(User user);
        Task<CacheStatus<int>> QueryCache();
    }
}