using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UAM.Kora
{
    partial class HashTable<T>
    {
        internal class InnerHashTable
        {
            // table-size is table.Length
            internal uint count;
            internal uint limit;
            internal KeyValuePair<uint, T>?[] table;
            // hashing arguments
            internal uint a;
            internal uint b;
            internal int width;

            internal InnerHashTable(uint length)
            {
                count = length;
                limit = 2 * length;
                uint hashSize = BitHacks.RoundToPower(2 * limit * (limit - 1));
                width = BitHacks.Power2MSB(hashSize);
                table = new KeyValuePair<uint, T>?[hashSize];
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
                count--;
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

            internal void RehashWith(uint key, T value, HashTable<T> parent, KeyValuePair<uint, T>?[] oldTable, int size)
            {
                var tempList = new KeyValuePair<uint, T>[count];
                for (int i = 0, j = 0; i < oldTable.Length; i++)
                {
                    if (oldTable[i] != null)
                    {
                        tempList[j] = oldTable[i].Value;
                        j++;
                    }
                }
                tempList[count-1] = new KeyValuePair<uint, T>(key, value);
                // we've got temp list ready, now try and find suitable function
                while (true)
                {
                    InitializeRandomHash(parent);
                    KeyValuePair<uint, T>?[] newTable = new KeyValuePair<uint, T>?[size];
                    // put them pairs where they belong
                    for (int i = 0; i < tempList.Length; i++)
                    {
                        uint index = GetHash(tempList[i].Key);
                        if (newTable[index] != null)
                            goto Failed;
                        newTable[index] = tempList[i];
                    }
                    table = newTable;
                    break;
                Failed:
                    continue;
                }
            }

            internal uint GetHash(uint x)
            {
#if TEST
                return ((a * x + b) % 997) % (uint)Math.Pow(2, this.width);
#else
            return ((a * x + b) >> (31 - width)) >> 1;
#endif
            }

            internal void InitializeRandomHash(HashTable<T> hashTable)
            {
                hashTable.InitializeRandomHash(out a, out b);
            }
        }
    }
}
