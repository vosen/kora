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
        uint pseudoCount;
        uint limit;
        internal InnerHashTable[] inner;
        internal uint a;
        internal uint b;
        internal int width;

        internal HashTable()
        {
            random = new Random();
            RehashAll(null);
        }

        // add overwrites
        public void Add(uint key, T value)
        {
            if (++pseudoCount > limit)
            {
                RehashAll(new KeyValuePair<uint, T>(key, value));
                return;
            }
            uint firstHash = GetHash(key);
            InnerHashTable innerHashed = inner[firstHash];
            uint secondHash = innerHashed.GetHash(key);
            // empty spot - just plop the value there and call it a day
            if (innerHashed.IsDeleted((int)secondHash))
            {
                innerHashed.count++;
                innerHashed.table[secondHash] = new KeyValuePair<uint, T>(key, value);
            }
            // replace the value
            else if (innerHashed.table[secondHash].Value.Key == key)
            {
                innerHashed.table[secondHash] = new KeyValuePair<uint, T>(key, value);
            }
            // We've got collision now, do something about it
            else
            {
                // Note that original algorithm does this in a roundabout way due to their weird data structures
                if (++innerHashed.count <= innerHashed.limit)
                {
                    // Rehash second level
                    innerHashed.RehashWith(key, value, this, innerHashed.table, innerHashed.table.Length);
                }
                else
                {
                    // Grow the second level
                    uint newLimit = limit = 2 * Math.Max(1, innerHashed.limit);
                    uint newSize = BitHacks.RoundToPower(2 * newLimit * (newLimit - 1));
                    innerHashed.limit = newLimit;
                    innerHashed.width = BitHacks.Power2MSB(newSize);
                    innerHashed.RehashWith(key, value, this, innerHashed.table, (int)newSize);
                }
            }
        }

        public void Remove(uint key)
        {
            pseudoCount++;
            uint firstHash = GetHash(key);
            uint secondHash = inner[firstHash].GetHash(key);
            if (inner[firstHash].table[secondHash] != null && inner[firstHash].table[secondHash].Value.Key == key)
            {
                inner[firstHash].RemoveAt((int)secondHash);
            }

            if (pseudoCount >= limit)
                RehashAll(null);
        }

        public bool TryGetValue(uint key, out T value)
        {
            uint firstHash = GetHash(key);
            uint secondHash = inner[firstHash].GetHash(key);
            if (!inner[firstHash].IsContained((int)secondHash))
            {
                value = default(T);
                return false;
            }
            var returnValue = inner[firstHash].table[secondHash];
            value = returnValue.Value.Value;
            return true;
        }

        internal uint GetHash(uint x)
        {
#if DEBUG
            return ((a * x + b) % 997) % (uint)Math.Pow(2, this.width);
#else
            return ((a * x + b) >> (31 - width)) >> 1;
#endif
        }

        /*
        internal Func<uint, uint> GetRandomHashMethod(uint size)
        {
            System.Diagnostics.Debug.Assert(size == BitHacks.RoundToPower(size));
            uint a = (uint)random.Next();
            uint b = (uint)(random.Next(65536) << 16);
            int shift = 31 - (int)BitHacks.Log2Ceiling(size);
            // weird shifting because c# can't shift uint by more than 31 bits
            return (x) =>  ((a * x + b) >> shift) >> 1;
        }
         * */

        private void RehashAll(KeyValuePair<uint, T>? newValue)
        {
            List<KeyValuePair<uint, T>> elements = new List<KeyValuePair<uint,T>>((int)(newValue == null ? pseudoCount : pseudoCount + 1));
            if(inner != null)
            {
                int j = 0;
                foreach(InnerHashTable table in inner)
                {
                    for(int i = 0; i< table.AllocatedSize; i++)
                    {
                        if (table.IsContained(i))
                        {
                            elements.Add(table.table[i].Value);
                            j++;
                        }
                    }
                }
                if(newValue.HasValue)
                    elements.Add(newValue.Value);
            }
            pseudoCount = (uint)elements.Count;
            float newLimit = (1.0f + Fill) * Math.Max(pseudoCount, 4.0f);
            limit = (uint)newLimit;
            // hashSize = s(M)
            uint hashSize = BitHacks.RoundToPower(limit << 1);
            width = BitHacks.Power2MSB(hashSize);
            List<KeyValuePair<uint, T>>[] hashList = null;
            // find suitable higher level function
            for(bool injective = false; !injective;)
            {
                InitializeRandomHash(out a, out b);
                hashList = new List<KeyValuePair<uint, T>>[hashSize];
                // initialize provisional list of elemnts going into second level table
                for (int i = 0; i < hashList.Length; i++)
                    hashList[i] = new List<KeyValuePair<uint,T>>();
                // run first level hashes
                foreach (var elm in elements)
                    hashList[GetHash(elm.Key)].Add(elm);
                var testTable = new InnerHashTable[hashSize];
                injective = SatisfiesMagicalCondition(hashList, limit);
            }
            // find suitable lower level function
            inner = new InnerHashTable[hashSize];
            for (int i = 0; i < hashSize; i++)
            {
                // We deviate from original algorithm here,
                // if we've got empty second level we initialize it to size one to avoid out-of-bounds access in other functions.
                inner[i] = new InnerHashTable(Math.Max((uint)hashList[i].Count,1));
                if (hashList[i].Count == 0)
                    inner[i].count = 0;
                while (true)
                {
                    inner[i].Clear();
                    inner[i].InitializeRandomHash(this);
                    for (int j = 0; j < hashList[i].Count; j++)
                    {
                        uint hash = inner[i].GetHash(hashList[i][j].Key);
                        if(inner[i].IsContained((int)hash))
                            // don't judge me
                            goto Failed;
                        inner[i].table[hash] = hashList[i][j];
                    }
                    break;
                Failed:
                    continue;
                }
            }
        }

        internal void InitializeRandomHash(out uint a, out uint b)
        {
#if DEBUG
            a = (uint)random.Next(1,997);
            b = (uint)random.Next(997);
#else
            a = ((uint)random.Next() * 2) + 1;
            b = (uint)(random.Next(65536) << 16);
#endif
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
