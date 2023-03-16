using ContactApi.Model;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Cache
{
    public interface ICacheHelper<T>
    {
        Task<T> GetOrCreateAsync(object key, Func<T> createItem);
        CacheStatus<T> GetStatus();
    }
}