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
        internal XFastTrie<RBTree> cluster;

        // removes new RBTree from the old tree
        internal static RBTree SplitNew(ref RBTree tree)
        {
            /* lazy split disabled - breaks the rb tree invariants
            if (tree.root.left != null)
                return LazySplitNew(tree);
             */
            RBTree.Node[] nodes = tree.ToSortedArray();
            tree = RBUtils.FromSortedList(nodes, (nodes.Length >> 1), nodes.Length - 1);
            return RBUtils.FromSortedList(nodes, 0, (nodes.Length >> 1) - 1);
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

        public YFastTrie()
        {
            cluster = new XFastTrie<RBTree>();
        }

        private XFastTrie<RBTree>.LeafNode Separator(uint key)
        {
            if (cluster.leafList == null)
                return null;
            // special check for maxval
            if (((XFastTrie<RBTree>.LeafNode)cluster.leafList.left).key == key)
                return (XFastTrie<RBTree>.LeafNode)cluster.leafList.left;
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
            var sep = Separator(key);
            if (sep == null)
            {
                value = default(T);
                return false;
            }
            RBUIntNode candidate = (RBUIntNode)sep.value.Lookup(key);
            if (candidate == null)
            {
                value = default(T);
                return false;
            }
            else 
            {
                value = candidate.value;
                return true;
            }
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
            var separator = Separator(key);
            if (separator == null || separator.value.Remove(key) == null)
                return false;
            // at this point key is removed from bst
            if (separator.left == separator || separator.value.Count >= lowerLimit)
                return true;
            // we need to rebuild
             XFastTrie<RBTree>.LeafNode adjacent;
            var left = (XFastTrie<RBTree>.LeafNode)separator.left;
            var right = (XFastTrie<RBTree>.LeafNode)separator.right;
            // pick best merge candidate
            if (left.key > separator.key)
                adjacent = right;
            else if (right.key < separator.key)
                adjacent = left;
            else 
                adjacent = left.value.Count <= right.value.Count ? left : right;
            if (!MergeSplit(separator, adjacent))
                cluster.Remove(adjacent.key);
            return true;
        }

        private static bool MergeSplit(XFastTrie<RBTree>.LeafNode primary, XFastTrie<RBTree>.LeafNode secondary)
        {
            XFastTrie<RBTree>.LeafNode lower = primary.key < secondary.key ? primary : secondary;
            XFastTrie<RBTree>.LeafNode higher = lower == primary ? secondary : primary;
            RBTree.Node[] array = new RBTree.Node[primary.value.Count + secondary.value.Count];
            lower.value.CopySorted(array, 0);
            higher.value.CopySorted(array, lower.value.Count);
            if (array.Length > upperLimit)
            {
                lower.value = RBUtils.FromSortedList(array, 0, array.Length >> 1);
                lower.key = ((RBUIntNode)array[array.Length >> 1]).key;
                higher.value = RBUtils.FromSortedList(array, (array.Length >> 1) + 1, array.Length - 1);
                higher.key = Math.Max(((RBUIntNode)array[array.Length - 1]).key, higher.key);
                return true;
            }
            else
            {
                primary.value = RBUtils.FromSortedList(array, 0, array.Length - 1);
                primary.key = Math.Max(((RBUIntNode)array[array.Length - 1]).key, higher.key);
                return false;
            }
        }

        public KeyValuePair<uint, T>? First()
        {
            var firstXNode = cluster.First();
            if (firstXNode == null)
                return null;
            var firstRBNode = (RBUIntNode)firstXNode.Value.Value.FirstNode();
            if (firstRBNode == null)
                return null;
            return new KeyValuePair<uint, T>(firstRBNode.key, firstRBNode.value);
        }

        public KeyValuePair<uint, T>? Last()
        {
            var lastXNode = cluster.Last();
            if (lastXNode == null)
                return null;
            var lastRBNode = (RBUIntNode)lastXNode.Value.Value.LastNode();
            if (lastRBNode == null)
                return null;
            return new KeyValuePair<uint, T>(lastRBNode.key, lastRBNode.value);
        }

        public KeyValuePair<uint, T>? Lower(uint key)
        {
            XFastTrie<RBTree>.LeafNode separator = Separator(key);
            if (separator == null)
                return null;
            RBUIntNode predNode = RBUtils.LowerNode(separator.value, key);
            if (predNode == null)
            {
                if (separator == cluster.leafList)
                    return null;
                RBUIntNode highestLeft = (RBUIntNode)((XFastTrie<RBTree>.LeafNode)separator.left).value.LastNode();
                return new KeyValuePair<uint, T>(highestLeft.key, highestLeft.value);
            }
            return new KeyValuePair<uint, T>(predNode.key, predNode.value);
        }

        public KeyValuePair<uint, T>? Higher(uint key)
        {
            XFastTrie<RBTree>.LeafNode higherNode = cluster.HigherNode(key);
            if (higherNode == null)
                return null;
            XFastTrie<RBTree>.LeafNode left = (XFastTrie<RBTree>.LeafNode)higherNode.left;
            if (((XFastTrie<RBTree>.LeafNode)higherNode.left).key == key)
            {
                var succNode = (RBUIntNode)higherNode.value.FirstNode();
                return new KeyValuePair<uint,T>(succNode.key, succNode.value);
            }
            RBUIntNode rbHigher = RBUtils.HigherNode(higherNode.value, key);
            if (rbHigher == null)
                return null;
            return new KeyValuePair<uint, T>(rbHigher.key, rbHigher.value);
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
