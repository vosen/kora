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

        public bool Remove(uint key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(uint key, out T value)
        {
            throw new NotImplementedException();
        }

        private Func<uint, uint> GetRandomHashMethod(uint size)
        {
            System.Diagnostics.Debug.Assert(size == BitHacks.RoundToPower(size));

            uint a = (uint)random.Next();
            uint b = (uint)(random.Next(65536) << 16);
            int shift = 32 - (int)size;
            return (x) => (a * x + b) >> shift;
        }

        private void RehashAll(uint? newValue)
        {
            uint[] elements = new uint[newValue == null ? count : count + 1];
            if(inner != null)
            {
                int j = 0;
                foreach(InnerHashTable table in inner)
                {
                    for(int i = 0; i< table.AllocatedSize; i++)
                    {
                        if (table.IsContained(i))
                        {
                            elements[j] = table[i];
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
            List<uint>[] hashList = null;
            // find suitable higher level function
            for(bool injective = false; !injective;)
            {
                function = GetRandomHashMethod(hashSize);
                hashList = new List<uint>[hashSize];
                for (int i = 0; i < hashList.Length; i++)
                {
                    hashList[i] = new List<uint>();
                }
                foreach (uint elm in elements)
                    hashList[function(elm)].Add(elm);
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
                        uint hash = inner[i].function(hashList[i][j]);
                        if(!inner[i].IsEmpty((int)hash))
                            goto Failed;
                        inner[i][j] = hashList[i][j];
                    }
                    injective = true;
                Failed:
                    continue;
                }
            }
        }

        private bool SatisfiesMagicalCondition(List<uint>[] inner, uint currLimit)
        {
            uint sum = 0;
            foreach (List<uint> tab in inner)
            {
                sum += (uint)((tab.Count << 3) - (4*tab.Count));
            }
            return sum <= ((32 * inner.Length * inner.Length) / currLimit) + 4 * inner.Length;
        }
    }
}
