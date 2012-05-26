using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UAM.Kora
{
    public partial class YFastTrie<T> : ISortedDictionary<uint, T>
    {
        const int upperLimit = 64;
        const int lowerLimit = 16;
        XFastTrie<RBTree> cluster;

        // removes new RBTree from the old tree
        internal static RBTree SplitNew(ref RBTree tree)
        {
            // try doing lazy split
            if (tree.root.left != null)
                return LazySplitNew(tree);
            RBTree.Node[] nodes = tree.ToSortedArray();
            tree = FromSortedList(nodes, (nodes.Length >> 1), nodes.Length - 1);
            return FromSortedList(nodes, 0, (nodes.Length >> 1) - 1);
        }

        internal static RBTree LazySplitNew(RBTree tree)
        {
            RBTree newTree = new RBTree(Node.Helper);
            newTree.root = tree.root.left;
            tree.root.left.IsBlack = true;
            tree.root.Size -= tree.root.left.Size;
            tree.root.left = null;
            return newTree;
        }

        internal static RBTree FromSortedList(RBTree.Node[] list, int start, int stop)
        {
            RBTree tree = new RBTree(new RBUIntNodeHelper());
            int length = stop - start + 1;
            if(start == stop)
                return tree;
            int maxDepth = BitHacks.Power2MSB(BitHacks.RoundToPower((uint)(length + 1))) - 1;
            tree.root = list[length >> 1];
            tree.root.IsBlack = true;
            tree.root.Size = (uint)length;
            RBInsertChildren(tree.root, true, 1, maxDepth, list, start, start + (length >> 1) - 1);
            RBInsertChildren(tree.root, false, 1, maxDepth, list, start + (length >> 1) + 1, stop);
            return tree;
        }

        static void RBInsertChildren(RBTree.Node node, bool left, int depth, int totalDepth, RBTree.Node[] list, int start, int stop)
        {
            if (start > stop)
                return;
            int middle = start + ((stop - start) >> 1);
            RBTree.Node current = list[middle];
            current.Size = (uint)(stop - start) + 1;
            current.IsBlack = (((totalDepth - depth) & 1) == 1);
            if(left)
                node.left = current;
            else
                node.right = current;
            RBInsertChildren(current, true, depth + 1, totalDepth, list, start, middle - 1);
            RBInsertChildren(current, false, depth + 1, totalDepth, list, middle + 1, stop);
        }

        public YFastTrie()
        {
            cluster = new XFastTrie<RBTree>();
        }

        private XFastTrie<RBTree>.LeafNode Separator(uint key)
        {
            var succ = cluster.HigherNode(key);
            if (succ == null)
                return null;
            XFastTrie<RBTree>.LeafNode left = (XFastTrie<RBTree>.LeafNode)succ.left;
            if (left.key == key)
                return left;
            return succ;
        }

        public void Add(uint key, T value)
        {
            var separator = Separator(key);
            if (separator == null)
            {
                // add first element
                RBTree newTree = new RBTree(Node.Helper);
                newTree.root = new RBUIntNode(key, value) { IsBlack = true };
                cluster.Add(uint.MaxValue, newTree);
                return;
            }
            RBUIntNode newNode = new RBUIntNode(key, value);
            if (separator.value.Intern(key, newNode) != newNode)
                throw new ArgumentException();
            SplitIfTooLarge(separator);
        }

        private void SplitIfTooLarge(XFastTrie<RBTree>.LeafNode separator)
        {
            if (separator.value.Count <= upperLimit)
                return;
            RBTree newTree = SplitNew(ref separator.value);
            cluster.Add(((RBUIntNode)newTree.LastNode()).key, newTree);
        }

        public bool TryGetValue(uint key, out T value)
        {
            var xSucc = cluster.HigherNode(key);
            if(xSucc == null)
            {
                value = default(T);
                return false;
            }
            XFastTrie<RBTree>.LeafNode left = (XFastTrie<RBTree>.LeafNode)xSucc.left;
            RBUIntNode candidate;
            if (left.key == key && left != xSucc)
            {
                candidate = (RBUIntNode)left.value.LastNode();
            }
            else
            {
                candidate = (RBUIntNode)xSucc.value.Lookup(key);
                if (candidate == null)
                {
                    value = default(T);
                    return false;
                }
            }
            if(candidate.key == key)
            {
                value = candidate.value;
                return true;
            }
            value = default(T);
            return false;
        }

        public T this[uint key]
        {
            get
            {
                T temp;
                if (!TryGetValue(key, out temp))
                    throw new KeyNotFoundException();
                return temp;
            }
            set
            {
                var separator = Separator(key);
                if (separator == null)
                {
                    // add first element
                    RBTree newTree = new RBTree(Node.Helper);
                    newTree.root = new RBUIntNode(key, value) { IsBlack = true };
                    cluster.Add(uint.MaxValue, newTree);
                    return;
                }
                var node = (RBUIntNode)separator.value.Intern(key, new RBUIntNode(key, value));
                node.value = value;
                SplitIfTooLarge(separator);
            }
        }


        public bool Remove(uint key)
        {
            throw new NotImplementedException();
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

        bool IDictionary<uint, T>.ContainsKey(uint key)
        {
            throw new NotImplementedException();
        }

        ICollection<uint> IDictionary<uint, T>.Keys
        {
            get { throw new NotImplementedException(); }
        }

        ICollection<T> IDictionary<uint, T>.Values
        {
            get { throw new NotImplementedException(); }
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
