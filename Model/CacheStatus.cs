using Microsoft.Extensions.Caching.Memory;
using Model;
using System.Collections;

namespace ContactApi.Model
{
    public class CacheStatus<T>
    {
        public long TotalHits { get; set; }
        public long TotalMisses { get; set; }
        public long CurrentEntryCount { get; set; }
        public List<KeyValuePair<T, object>> CurrentEntry { get; set; }
        public IEnumerable<Tuple<T, DateTime>> FrequencyHits { get; set; }
    }
}
