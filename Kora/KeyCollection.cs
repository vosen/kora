using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UAM.Kora
{
    public class KeyCollection<T> : ICollection<uint>
    {
        private IDictionary<uint, T> tree;

        internal KeyCollection(IDictionary<uint, T> dict)
        {
            tree = dict;
        }

        public IEnumerator<uint> GetEnumerator()
        {
            var iter = tree.GetEnumerator();
            while (iter.MoveNext())
                yield return iter.Current.Key;
        }

        public int Count
        {
            get { return tree.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        #region implicits

        void ICollection<uint>.Add(uint item)
        {
            throw new NotSupportedException();
        }

        void ICollection<uint>.Clear()
        {
            throw new NotSupportedException();
        }

        bool ICollection<uint>.Contains(uint item)
        {
            return tree.ContainsKey(item);
        }

        void ICollection<uint>.CopyTo(uint[] array, int arrayIndex)
        {
            ICollectionHelpers.ThrowIfInsufficientArray(this, array, arrayIndex);
            var iter = tree.GetEnumerator();
            for (int i = 0; i < tree.Count; i++)
            {
                iter.MoveNext();
                array[i] = iter.Current.Key;
            }
        }

        bool ICollection<uint>.Remove(uint item)
        {
            throw new NotSupportedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

    }
}
