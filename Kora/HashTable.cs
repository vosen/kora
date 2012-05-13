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
        uint version;
        uint limit;
        InnerHashTable[] inner;
        Func<uint, uint> function;

        internal HashTable()
        {
            random = new Random();
            RehashAll(null);
        }

        // add overwrites
        public void Add(uint key, T value)
        {
            if (++version > limit)
            {
                RehashAll(new KeyValuePair<uint, T>(key, value));
                return;
            }
            uint firstHash = function(key);
            InnerHashTable innerHashed = inner[firstHash];
            uint secondHash = innerHashed.function(key);
            if (innerHashed.IsDeleted((int)secondHash))
            {
                innerHashed.table[secondHash] = new KeyValuePair<uint, T>(key, value);
                return;
            }
            // We've got collision now, do something about it
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
                uint newSize = 2 * newLimit * (newLimit -1);
                innerHashed.limit = newLimit;
                innerHashed.RehashWith(key, value, this, innerHashed.table, (int)newSize);
            }
        }

        public void Remove(uint key)
        {
            version++;
            uint firstHash = function(key);
            uint secondHash = inner[firstHash].function(key);
            if (inner[firstHash].IsContained((int)secondHash))
            {
                inner[firstHash].RemoveAt((int)secondHash);
            }

            if (version >= limit)
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
            var returnValue = inner[firstHash].table[secondHash];
            value = returnValue.Value.Value;
            return true;
        }

        internal Func<uint, uint> GetRandomHashMethod(uint size)
        {
            System.Diagnostics.Debug.Assert(size == BitHacks.RoundToPower(size));

            uint a = (uint)random.Next();
            uint b = (uint)(random.Next(65536) << 16);
            int shift = 31 - (int)BitHacks.Log2Ceiling(size);
            // weird shifting because c# can shift 32 bit values by up to 31 bits
            return (x) =>  ((a * x + b) >> shift) >> 1;
        }

        private void RehashAll(KeyValuePair<uint, T>? newValue)
        {
            KeyValuePair<uint, T>[] elements = new KeyValuePair<uint,T>[newValue == null ? version : version + 1];
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
            version = (uint)elements.LongLength;
            float newLimit = (1.0f + Fill) * Math.Max(version, 4.0f);
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
                inner[i] = new InnerHashTable(Math.Max((uint)hashList[i].Count,1));
                while (true)
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
                    break;
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
