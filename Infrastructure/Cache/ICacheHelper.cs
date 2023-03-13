using ContactApi.Model;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Cache
{
    public interface ICacheHelper<T>
    {
        T GetOrCreate(object key, Func<T> createItem);
        CacheStatus<T> GetStatus();
    }
}