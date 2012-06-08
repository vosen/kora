using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UAM.Kora
{
    public partial class YFastTrie<T> : SortedDictionaryBase<T>
    {
        private int upperLimit;
        private int lowerLimit;
        int count;
        uint version;
        internal XFastTrie<RBTree> cluster;

        // removes new RBTree from the old tree
        internal static RBTree SplitNew(ref RBTree tree)
        {
            RBTree.Node[] nodes = tree.ToSortedArray();
            tree = RBUtils.FromSortedList(nodes, (nodes.Length >> 1), nodes.Length - 1);
            return RBUtils.FromSortedList(nodes, 0, (nodes.Length >> 1) - 1);
        }

        private YFastTrie(XFastTrie<RBTree> newTree)
        {
            cluster = newTree;
            lowerLimit = (cluster.width / 2);
            upperLimit = (cluster.width * 2);
        }

        public YFastTrie()
            : this(new XFastTrie<RBTree>())
        { }

        public YFastTrie(int width)
            : this(new XFastTrie<RBTree>(width))
        {}

        public static YFastTrie<T> FromDictionary<TDict>(int width) where TDict : IDictionary<uint, XFastNode>, new()
        {
            return new YFastTrie<T>(XFastTrie<RBTree>.FromDictionary<TDict>(width));
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

        private void AddChecked(uint key, T value, bool overwrite)
        {
            
            var separator = Separator(key);
            if (separator == null)
            {
                // add first element
                RBTree newTree = new RBTree(Node.Helper);
                newTree.root = new RBUIntNode(key, value) { IsBlack = true };
                cluster.Add(BitHacks.MaxValue(cluster.width), newTree);
                return;
            }
            RBUIntNode newNode = new RBUIntNode(key, value);
            RBUIntNode interned = (RBUIntNode)separator.value.Intern(key, newNode);
            if (interned != newNode)
            {
                if (overwrite)
                    interned.value = value;
                else
                    throw new ArgumentException();
            }
            else
            {
                count++;
                version++;
            }
            SplitIfTooLarge(separator);
        }

        public override void Add(uint key, T value)
        {
            AddChecked(key, value, false);
        }

        private void SplitIfTooLarge(XFastTrie<RBTree>.LeafNode separator)
        {
            if (separator.value.Count <= upperLimit)
                return;
            RBTree newTree = SplitNew(ref separator.value);
            cluster.Add(((RBUIntNode)newTree.LastNode()).key, newTree);
        }

        public override bool TryGetValue(uint key, out T value)
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

        public override T this[uint key]
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
                AddChecked(key, value, true);
            }
        }

        public override bool Remove(uint key)
        {
            var separator = Separator(key);
            // TODO: remove this check
            if (separator != null && !cluster.ContainsKey(separator.key))
                throw new Exception();
            if (separator == null || separator.value.Remove(key) == null)
                return false;
            count--;
            version++;
            // at this point key is removed from bst
            if (separator.left == separator || separator.value.Count >= lowerLimit)
                return true;
            // we need to rebuild
            XFastTrie<RBTree>.LeafNode lower;
            XFastTrie<RBTree>.LeafNode higher;
            var left = (XFastTrie<RBTree>.LeafNode)separator.left;
            var right = (XFastTrie<RBTree>.LeafNode)separator.right;
            // pick best merge candidate
            if (left.key > separator.key)
            {
                // we are the first leaf
                lower = separator;
                higher = right;
            }
            else if (right.key < separator.key)
            {
                // we are the last leaf
                lower = left;
                higher = separator;
            }
            // separate branch, we want to avoid situation where we compare count of
            // leaf on the other side of linked list's circle
            else if (left.value.Count < right.value.Count)
            {
                lower = left;
                higher = separator;
            }
            else
            {
                lower = separator;
                higher = right;
            }
            if (MergeSplit(ref higher.value, ref lower.value))
            {
                // Split happened: reinsert lower key
                System.Diagnostics.Debug.Assert(cluster.Remove(lower.key));
                lower.key = ((RBUIntNode)lower.value.LastNode()).key;
                cluster.Add(lower.key, lower.value);
            }
            else
            {
                // Only merge happened: just remove lower node
                cluster.Remove(lower.key);
            }
            return true;
        }

        private bool MergeSplit(ref RBTree higher, ref RBTree lower)
        {
            RBTree.Node[] array = new RBTree.Node[higher.Count + lower.Count];
            lower.CopySorted(array, 0);
            higher.CopySorted(array, lower.Count);
            if (array.Length > upperLimit)
            {
                lower = RBUtils.FromSortedList(array, 0, array.Length >> 1);
                higher = RBUtils.FromSortedList(array, (array.Length >> 1) + 1, array.Length - 1);
                return true;
            }
            else
            {
                higher = RBUtils.FromSortedList(array, 0, array.Length - 1);
                return false;
            }
        }

        public override KeyValuePair<uint, T>? First()
        {
            var firstXNode = cluster.First();
            if (firstXNode == null)
                return null;
            var firstRBNode = (RBUIntNode)firstXNode.Value.Value.FirstNode();
            if (firstRBNode == null)
                return null;
            return new KeyValuePair<uint, T>(firstRBNode.key, firstRBNode.value);
        }

        public override KeyValuePair<uint, T>? Last()
        {
            var lastXNode = cluster.Last();
            if (lastXNode == null)
                return null;
            var lastRBNode = (RBUIntNode)lastXNode.Value.Value.LastNode();
            if (lastRBNode == null)
                return null;
            return new KeyValuePair<uint, T>(lastRBNode.key, lastRBNode.value);
        }

        public override KeyValuePair<uint, T>? Lower(uint key)
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

        public override KeyValuePair<uint, T>? Higher(uint key)
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

        public override void Clear()
        {
            count = 0;
            version = 0;
            cluster = new XFastTrie<RBTree>();
        }

        public override int Count
        {
            get { return count; }
        }

        public override bool ContainsKey(uint key)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<KeyValuePair<uint, T>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

#if DEBUG
        public void Verify()
        {
            cluster.Verify();
            foreach (var node in cluster)
            {
                node.Value.VerifyInvariants();
            }
        }
#endif
    }
}
