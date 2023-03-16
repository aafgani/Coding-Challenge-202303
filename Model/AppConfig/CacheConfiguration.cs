using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.AppConfig
{
    public class CacheConfiguration
    {
        public int MaxEntry { get; set; }
        public int AbsoluteExpiration { get; set; }
    }
}
