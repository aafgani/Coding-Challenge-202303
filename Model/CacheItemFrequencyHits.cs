using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CacheItemFrequencyHits<T>
    {
        public T Key { get; set; }
        public List<DateTime> HitsAt { get; set; }
    }
}
