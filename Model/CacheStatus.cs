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
        public List<IGrouping<object, Tuple<object, DateTime>>> FrequencyHits { get; set; }
    }
}
