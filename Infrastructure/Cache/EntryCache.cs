using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Cache
{
    public class EntryCache<T>
    {
        public T Item { get; set; }
        public TimeSpan AbsoluteExpiration { get; set; }
        public DateTime CreatedTime { get; set; }

    }
}
