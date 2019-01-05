using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeBeats.Other
{
    public class DDictionary<K,V> : Dictionary<K, V>
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

        public void AddRange(Dictionary<K, V> dic)
        {
            foreach (var dicPair in dic)
            {
                Add(dicPair);
            }
        }

        public void AddRange(List<KeyValuePair<K, V>> list)
        {
            foreach (var dicPair in list)
            {
                Add(dicPair);
            }
        }

        public void Add(KeyValuePair<K,V> pair)
        {
            base.Add(pair.Key, pair.Value);
        }
    }
}
