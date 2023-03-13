using Model;

namespace Infrastructure.HttpClientService
{
    public interface IWebCacheServiceClient
    {
        Task<int> CreateUser(User user);
    }
}