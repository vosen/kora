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
            private uint[] table;
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
                table = new uint[hashSize];
                control = new byte[hashSize];
            }

            internal void Clear()
            {
                table = new uint[table.Length];
            }

            internal uint this[int i]
            {
                get
                {
                    System.Diagnostics.Debug.Assert(IsContained(i));
                    return table[i];
                }
                set
                {
                    table[i] = value;
                    control[i] = 1;
                }
            }

            internal uint AllocatedSize
            {
                get { return (uint)table.Length; }
            }

            internal void RemoveAt(int idx)
            {
                control[idx] = 2;
            }

            internal bool IsEmpty(int idx)
            {
                return control[idx] == 0;
            }

            internal bool IsContained(int idx)
            {
                return control[idx] == 1;
            }

            internal bool IsDeleted(int idx)
            {
                return control[idx] == 2;
            }
        }
    }
}
