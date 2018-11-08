using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeBeats.Other
{
    public class NDictionary<K,V> : Dictionary<K, V>
    {
        public new V this[K key]
        {
            get
            {
                if (base.ContainsKey(key))
                    return base[key];
                else
                    return default(V);
            }
        }
    }
}
