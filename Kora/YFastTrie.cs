using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UAM.Kora
{
    public partial class YFastTrie<T> : ISortedDictionary<uint, T>
    {
        internal static RBTree FromSortedList(uint[] list)
        {
            RBTree tree = new RBTree(new RBUIntNodeHelper());
            if(list.Length == 0)
                return tree;
            int maxDepth = BitHacks.Power2MSB(BitHacks.RoundToPower((uint)list.Length + 1)) - 1;
            tree.root = new RBUIntNode(list[list.Length >> 1]) { IsBlack = true, Size = (uint)list.Length };
            RBInsertChildren(tree.root, true, 1, maxDepth, list, 0, (list.Length >> 1) - 1);
            RBInsertChildren(tree.root, false, 1, maxDepth, list, (list.Length >> 1) + 1, list.Length - 1);
            return tree;
        }

        static void RBInsertChildren(RBTree.Node node, bool left, int depth, int totalDepth, uint[] list, int start, int stop)
        {
            if (start > stop)
                return;
            int middle = start + ((stop - start) >> 1);
            RBUIntNode newNode = new RBUIntNode(list[middle]) { Size = (uint)(stop - start) + 1 };
            newNode.IsBlack = (((totalDepth - depth) & 1) == 1);
            if(left)
                node.left = newNode;
            else
                node.right = newNode;
            RBInsertChildren(newNode, true, depth + 1, totalDepth, list, start, middle - 1);
            RBInsertChildren(newNode, false, depth + 1, totalDepth, list, middle + 1, stop);
        }

        KeyValuePair<uint, T>? ISortedDictionary<uint, T>.First()
        {
            throw new NotImplementedException();
        }

        KeyValuePair<uint, T>? ISortedDictionary<uint, T>.Last()
        {
            throw new NotImplementedException();
        }

        KeyValuePair<uint, T>? ISortedDictionary<uint, T>.Lower(uint key)
        {
            throw new NotImplementedException();
        }

        KeyValuePair<uint, T>? ISortedDictionary<uint, T>.Higher(uint key)
        {
            throw new NotImplementedException();
        }

        void IDictionary<uint, T>.Add(uint key, T value)
        {
            throw new NotImplementedException();
        }

        bool IDictionary<uint, T>.ContainsKey(uint key)
        {
            throw new NotImplementedException();
        }

        ICollection<uint> IDictionary<uint, T>.Keys
        {
            get { throw new NotImplementedException(); }
        }

        bool IDictionary<uint, T>.Remove(uint key)
        {
            throw new NotImplementedException();
        }

        bool IDictionary<uint, T>.TryGetValue(uint key, out T value)
        {
            throw new NotImplementedException();
        }

        ICollection<T> IDictionary<uint, T>.Values
        {
            get { throw new NotImplementedException(); }
        }

        T IDictionary<uint, T>.this[uint key]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        void ICollection<KeyValuePair<uint, T>>.Add(KeyValuePair<uint, T> item)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<uint, T>>.Clear()
        {
            throw new NotImplementedException();
        }

        bool ICollection<KeyValuePair<uint, T>>.Contains(KeyValuePair<uint, T> item)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<uint, T>>.CopyTo(KeyValuePair<uint, T>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        int ICollection<KeyValuePair<uint, T>>.Count
        {
            get { throw new NotImplementedException(); }
        }

        bool ICollection<KeyValuePair<uint, T>>.IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        bool ICollection<KeyValuePair<uint, T>>.Remove(KeyValuePair<uint, T> item)
        {
            throw new NotImplementedException();
        }

        IEnumerator<KeyValuePair<uint, T>> IEnumerable<KeyValuePair<uint, T>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
