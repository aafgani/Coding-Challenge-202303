﻿using ContactApi.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Model;
using Model.AppConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Cache
{
    public class MyOwnCacheHelper<T> : ICacheHelper<T>
    {
        private static readonly SemaphoreSlim GetUsersSemaphore = new SemaphoreSlim(1, 1);
        private readonly Dictionary<object, EntryCache<T>> _cache;
        private readonly List<Tuple<object, DateTime>> _hits;
        private readonly List<Tuple<object, DateTime>> _missed;
        private readonly int _maxEntry;
        private readonly int _absoluteExpirationInMiliseconds;

        public MyOwnCacheHelper(IOptions<CacheConfiguration> options)
        {
            _cache = new Dictionary<object, EntryCache<T>>();
            _hits = new List<Tuple<object, DateTime>>();
            _missed = new List<Tuple<object, DateTime>>();
            _maxEntry = options.Value.MaxEntry;
            _absoluteExpirationInMiliseconds = options.Value.AbsoluteExpiration;
        }

        private async Task<T> GetOrCreateAsync(object key, Func<T> createItem, SemaphoreSlim semaphore)
        {
            EntryCache<T> cacheEntry;
            var currentEntryCount = _cache.Count;

            try
            {
                await semaphore.WaitAsync();
                if (!_cache.TryGetValue(key, out cacheEntry))
                {
                    var item = createItem();
                    cacheEntry = new EntryCache<T>
                    {
                        Item = item,
                        AbsoluteExpiration = TimeSpan.FromMilliseconds(_absoluteExpirationInMiliseconds),
                        CreatedTime = DateTime.Now,
                    };
                    _missed.Add(Tuple.Create(key, DateTime.Now));

                    if (currentEntryCount < _maxEntry || CheckIfAnyExpiration())
                    {
                        _cache.Add(key, cacheEntry);
                    }
                }
                else
                {
                    _hits.Add(Tuple.Create(key, DateTime.Now));
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                semaphore.Release();
            }    
            return cacheEntry.Item;
        }

        public async Task<T> GetOrCreateAsync(object key, Func<T> createItem)
        {
            try
            {
                return await GetOrCreateAsync(key, createItem, GetUsersSemaphore);
            }
            catch
            {
                throw;
            }
        }

        public CacheStatus<T> GetStatus()
        {
            var stats = new CacheStatus<T>();
            stats.TotalHits = _hits.Count;
            stats.TotalMisses = _missed.Count;
            stats.CurrentEntryCount =_cache.Count;
            stats.FrequencyHits = _hits
                 .Where(i => i.Item2 > DateTime.Now.AddSeconds(-5))
                 .GroupBy(i => i.Item1)
                 .ToList();
            return stats;
        }

        private bool CheckIfAnyExpiration()
        {
            var anyExpired = _cache.Where(i => i.Value.CreatedTime.Add(i.Value.AbsoluteExpiration) < DateTime.Now).ToList();
            anyExpired.ForEach(i =>
            {
                _cache.Remove(i.Key);
            });

            return anyExpired.Any();
        }
    }
}
