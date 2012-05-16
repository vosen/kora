using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UAM.Kora
{
    partial class XFastTrie<T> : ISortedDictionary<uint, T>
    {
        private static int width = 32;
        int count;
        int version;
        HashTable<Node>[] table;
        LeafNode leafList;

        public XFastTrie()
        {
            table = new HashTable<Node>[width];
            for (int i = 0; i < width; i++)
                table[i] = new HashTable<Node>();
        }

        private Node Bottom(uint key)
        {
            int l = 0;
            int h = width;
            Node tempNode;
            Node correctNode = null;
            do
            {
                int j = (l + h) / 2;
                uint ancestor = key >> (width - j);
                if (table[j].TryGetValue(ancestor, out tempNode))
                {
                    l = j;
                    correctNode = tempNode;
                }
                else
                {
                    h = j;
                }
            }
            while (h - l > 1);
            return correctNode;
        }

        private void InsertLeafAfter(Node marker, LeafNode newLeaf)
        {
            if (marker == null)
            {
                leafList = newLeaf;
                newLeaf.left = newLeaf;
                newLeaf.right = newLeaf;
                return;
            }
            Node rightNode = marker.right;
            marker.right = newLeaf;
            newLeaf.left = marker;
            newLeaf.right = rightNode;
            rightNode.left = newLeaf;
        }

        public KeyValuePair<uint, T>? First()
        {
            if (leafList == null)
                return null;
            return new KeyValuePair<uint,T>(leafList.key, leafList.value);
        }

        public KeyValuePair<uint, T>? Last()
        {
            if (leafList == null)
                return null;
            if (leafList.left == leafList)
                return new KeyValuePair<uint, T>(leafList.key, leafList.value);
            LeafNode leftLeaf = (LeafNode)leafList.left;
            return new KeyValuePair<uint, T>(leftLeaf.key, leftLeaf.value);
        }

        private LeafNode LowerNodeFromBottom(Node bottom, uint key)
        {
            if (bottom == null)
                return null;
            LeafNode leaf = bottom.right as LeafNode;
            if (leaf != null && leaf.key < key)
                return leaf;
            leaf = bottom.left as LeafNode;
            if (leaf != null && leaf.key < key)
                return leaf;
            if (bottom.left != null)
            {
                leaf = bottom.left.left as LeafNode;
                if (leaf != null && leaf.key < key)
                    return leaf;
            }
            return null;
        }

        private LeafNode LowerNode(uint key)
        {
            Node ancestor = Bottom(key);
            return LowerNodeFromBottom(ancestor, key);
        }

        private LeafNode HigherNode(uint key)
        {
            Node ancestor = Bottom(key);
            if (ancestor == null)
                return null;
            LeafNode leaf = ancestor.left as LeafNode;
            if (leaf != null && leaf.key > key)
                return leaf;
            leaf = ancestor.right as LeafNode;
            if (leaf != null && leaf.key > key)
                return leaf;
            if (ancestor.right != null)
            {
                leaf = ancestor.right.right as LeafNode;
                if (leaf != null && leaf.key > key)
                    return leaf;
            }
            return null;
        }

        public KeyValuePair<uint, T>? Lower(uint key)
        {
            var lower = LowerNode(key);
            if (lower == null)
                return null;
            return new KeyValuePair<uint,T>(lower.key, lower.value);
        }

        public KeyValuePair<uint, T>? Higher(uint key)
        {
            var lower = HigherNode(key);
            if (lower == null)
                return null;
            return new KeyValuePair<uint, T>(lower.key, lower.value);
        }

        private void AddChecked(uint key, T value, bool overwrite)
        {
            // Insert node in linked list
            Node bottom = Bottom(key);
            LeafNode predecessor = LowerNodeFromBottom(bottom, key);
            // check for overwrite
            if (predecessor != null)
            {
                LeafNode predRight = (LeafNode)predecessor.right;
                if (predRight.key == key)
                {
                    if (!overwrite)
                        throw new ArgumentException();
                    else
                    {
                        predRight.value = value;
                        return;
                    }
                }
            }
            // merrily continue
            LeafNode endNode = new LeafNode() { key = key, value = value };
            InsertLeafAfter(predecessor, endNode);
            // Fix the jump path
            if (bottom == null)
            {
                bottom = new Node();
                table[0].Add(0, bottom);
                bottom.left = endNode;
                bottom.right = endNode;
            }

            Node oldNode = null;
            Node current;
            for (int i = 0; i < width; i++)
            {
                uint id = key >> (width - 1 - i) >> 1;
                if (table[i].TryGetValue(id, out current))
                {
                    // fix the jump path
                    LeafNode leaf = current.left as LeafNode;
                    if (leaf != null && leaf.key > key)
                    {
                        current.left = endNode;
                    }
                    else
                    {
                        leaf = current.right as LeafNode;
                        if(leaf != null && leaf.key < key)
                            current.right = endNode;
                    }
                }
                else
                {
                    // insert new node
                    current = new Node() { left = endNode, right = endNode };
                    table[i].Add(id, current);
                    // fix link between old and new node
                    if ((id & 1) > 0)
                        oldNode.right = current;
                    else
                        oldNode.left = current;
                }
                oldNode = current;
            }
        }

        public void Add(uint key, T value)
        {
            AddChecked(key, value, false);
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

        public bool TryGetValue(uint key, out T value)
        {
            Node ancestor = Bottom(key);
            if (ancestor == null)
            {
                value = default(T);
                return false;
            }

            LeafNode leaf = ancestor.left as LeafNode;
            if (leaf != null && leaf.key == key)
            {
                value = leaf.value;
                return true;
            }

            leaf = ancestor.right as LeafNode;
            if (leaf != null && leaf.key == key)
            {
                value = leaf.value;
                return true;
            }

            value = default(T);
            return false;
        }

        ICollection<T> IDictionary<uint, T>.Values
        {
            get { throw new NotImplementedException(); }
        }

        public T this[uint key]
        {
            get
            {
                T temp;
                if (TryGetValue(key, out temp))
                    return temp;
                else
                    throw new KeyNotFoundException();
            }
            set
            {
                AddChecked(key, value, true);
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
