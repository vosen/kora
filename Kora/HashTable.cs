using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UAM.Kora
{
    internal partial class HashTable<T>
    {
        private static float Fill = 0.66f;
        // SetSize ~= ((8/15)*sqrt(30)) / 1,5
        //private static uint SetSize = 2;

        Random random;
        uint count;
        uint limit;
        InnerHashTable[] inner;
        Func<uint, uint> function;

        internal HashTable()
        {
            random = new Random();
            RehashAll(null);
        }

        public void Add(uint key, T value)
        {
            throw new NotImplementedException();
        }

        internal uint Count
        {
            get { return count; }
        }

        public void Remove(uint key)
        {
            count++;
            uint firstHash = function(key);
            uint secondHash = inner[firstHash].function(key);
            if (inner[firstHash].IsContained((int)secondHash))
            {
                inner[firstHash].RemoveAt((int)secondHash);
            }

            if (count >= limit)
                RehashAll(null);
        }

        public bool TryGetValue(uint key, ref T value)
        {
            uint firstHash = function(key);
            uint secondHash = inner[firstHash].function(key);
            if (!inner[firstHash].IsContained((int)secondHash))
            {
                return false;
            }
            value = inner[firstHash].values[secondHash];
            return true;
        }

        private Func<uint, uint> GetRandomHashMethod(uint size)
        {
            System.Diagnostics.Debug.Assert(size == BitHacks.RoundToPower(size));

            uint a = (uint)random.Next();
            uint b = (uint)(random.Next(65536) << 16);
            int shift = 32 - (int)size;
            return (x) => (a * x + b) >> shift;
        }

        private void RehashAll(KeyValuePair<uint, T>? newValue)
        {
            KeyValuePair<uint, T>[] elements = new KeyValuePair<uint,T>[newValue == null ? count : count + 1];
            if(inner != null)
            {
                int j = 0;
                foreach(InnerHashTable table in inner)
                {
                    for(int i = 0; i< table.AllocatedSize; i++)
                    {
                        if (table.IsContained(i))
                        {
                            elements[j] = table.table[i].Value;
                            j++;
                        }
                    }
                }
                if(newValue.HasValue)
                    elements[j] = newValue.Value;
            }
            count = (uint)elements.LongLength;
            float newLimit = (1.0f + Fill) * Math.Max(count, 4.0f);
            limit = (uint)newLimit;
            // hashSize = s(M)
            uint hashSize = BitHacks.RoundToPower(limit << 1);
            List<KeyValuePair<uint, T>>[] hashList = null;
            // find suitable higher level function
            for(bool injective = false; !injective;)
            {
                function = GetRandomHashMethod(hashSize);
                hashList = new List<KeyValuePair<uint, T>>[hashSize];
                // initialize provisional list of eleemnts going into second level table
                for (int i = 0; i < hashList.Length; i++)
                    hashList[i] = new List<KeyValuePair<uint,T>>();
                // run first level hashes
                foreach (var elm in elements)
                    hashList[function(elm.Key)].Add(elm);
                var testTable = new InnerHashTable[hashSize];
                injective = SatisfiesMagicalCondition(hashList, limit);
            }
            // find suitable lower level function
            inner = new InnerHashTable[hashSize];
            for (int i = 0; i < hashSize; i++)
            {
                inner[i] = new InnerHashTable((uint)hashList[i].Count);
                for (bool injective = false; !injective; )
                {
                    inner[i].Clear();
                    inner[i].function = GetRandomHashMethod(inner[i].AllocatedSize);
                    for (int j = 0; j < hashList[i].Count; j++)
                    {
                        uint hash = inner[i].function(hashList[i][j].Key);
                        if(inner[i].IsContained((int)hash))
                            // don't judge me
                            goto Failed;
                        inner[i].table[j] = hashList[i][j];
                    }
                    injective = true;
                Failed:
                    continue;
                }
            }
        }

        private bool SatisfiesMagicalCondition(List<KeyValuePair<uint,T>>[] inner, uint currLimit)
        {
            uint sum = 0;
            foreach (var tab in inner)
            {
                sum += (uint)((tab.Count << 3) - (4*tab.Count));
            }
            return sum <= ((32 * inner.Length * inner.Length) / currLimit) + 4 * inner.Length;
        }
    }
}
