using ContactApi.Model;
using Microsoft.Extensions.Caching.Memory;
using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Cache
{
    public class CacheHelper<T> : ICacheHelper<T>
    {
        private readonly IMemoryCache memoryCache;
        private readonly int maxEntry = 100;
        private readonly List<CacheItemFrequencyHits<T>> cacheItemFrequencyHits;
        private int a = 0;
        public CacheHelper()
        {
            this.memoryCache = new MemoryCache(
                new MemoryCacheOptions
                {
                    TrackStatistics = true,
                });
            cacheItemFrequencyHits = new List<CacheItemFrequencyHits<T>>();
        }

        public T GetOrCreate(object key, Func<T> createItem)
        {
            T cacheEntry;
            var currentEntryCount = memoryCache.GetCurrentStatistics()?.CurrentEntryCount;
            if (!memoryCache.TryGetValue(key, out cacheEntry))
            {
                if (currentEntryCount < maxEntry)
                {
                    cacheEntry = createItem();
                    memoryCache.Set(key, cacheEntry, TimeSpan.FromMinutes(1));

                    cacheItemFrequencyHits.Add(new CacheItemFrequencyHits<T>
                    {
                        Key = cacheEntry,
                        HitsAt = new List<DateTime>
                        {
                            DateTime.Now,
                        }
                    });
                }
            }
            else
            {
                var cacheItem = cacheItemFrequencyHits.Where(i => i.Key.Equals(cacheEntry)).FirstOrDefault();
                cacheItem.HitsAt.Add(DateTime.Now);
            }
            return cacheEntry;
        }

        public CacheStatus<T> GetStatus()
        {
            var stats = new CacheStatus<T>();
            var coherentState = typeof(MemoryCache).GetField("_coherentState", BindingFlags.NonPublic | BindingFlags.Instance);

            var coherentStateValue = coherentState.GetValue(memoryCache);

            var entriesCollection = coherentStateValue.GetType().GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);

            var entriesCollectionValue = entriesCollection.GetValue(coherentStateValue) as ICollection;

            var s = memoryCache.GetCurrentStatistics();

            stats.TotalHits = s.TotalHits;
            stats.TotalMisses = s.TotalMisses;
            stats.CurrentEntryCount = s.CurrentEntryCount;

            return stats;
        }

    }
}
