using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UAM.Kora
{
    public interface ISortedDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        KeyValuePair<TKey, TValue>? First();
        KeyValuePair<TKey, TValue>? Last();
        KeyValuePair<TKey, TValue>? Lower(TKey key);
        KeyValuePair<TKey, TValue>? Higher(TKey key);
    }
}
