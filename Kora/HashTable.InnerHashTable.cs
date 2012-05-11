using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UAM.Kora
{
    internal partial class HashTable<T>
    {
        private class InnerHashTable
        {
            // table-size is table.Length
            private uint count;
            private uint limit;
            internal KeyValuePair<uint, T>?[] table;
            internal T[] values;
            // 0 is for empty
            // 1 is for contained
            // 2 is for deleted
            private byte[] control;
            internal Func<uint, uint> function;

            internal InnerHashTable(uint length)
            {
                count = length;
                limit = 2 * length;
                uint hashSize = BitHacks.RoundToPower(2 * limit * (limit - 1));
                table = new KeyValuePair<uint, T>?[hashSize];
                control = new byte[hashSize];
            }

            internal void Clear()
            {
                table = new KeyValuePair<uint, T>?[table.Length];
            }

            internal uint AllocatedSize
            {
                get { return (uint)table.Length; }
            }

            internal void RemoveAt(int idx)
            {
                table[idx] = null;
            }

            internal bool IsDeleted(int idx)
            {
                return table[idx] == null;
            }

            internal bool IsContained(int idx)
            {
                return table[idx] != null;
            }
        }
    }
}
