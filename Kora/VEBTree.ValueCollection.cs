using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UAM.Kora
{
    public partial class VEBTree<T>
    {
        public class ValueCollection : ICollection<T>
        {
            private VEBTree<T> tree;

            internal ValueCollection(VEBTree<T> vebTree)
            {
                tree = vebTree;
            }

            public IEnumerator<T> GetEnumerator()
            {
                var iter = tree.GetEnumerator();
                while (iter.MoveNext())
                    yield return iter.Current.Value;
            }

            public bool IsReadOnly
            {
                get { return true; }
            }

            #region implicits

            int ICollection<T>.Count
            {
                get { return tree.Count; }
            }

            void ICollection<T>.Add(T item)
            {
                throw new NotSupportedException();
            }

            void ICollection<T>.Clear()
            {
                throw new NotSupportedException();
            }

            bool ICollection<T>.Contains(T item)
            {
                return tree.Any(kvp => kvp.Value.Equals(item));
            }

            void ICollection<T>.CopyTo(T[] array, int arrayIndex)
            {
                ICollectionHelpers.ThrowIfInsufficientArray(this, array, arrayIndex);
                var iter = tree.GetEnumerator();
                for (int i = 0; i < tree.Count; i++)
                {
                    iter.MoveNext();
                    array[i] = iter.Current.Value;
                }
            }

            bool ICollection<T>.Remove(T item)
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
}
