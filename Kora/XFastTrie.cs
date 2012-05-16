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
            Node node = null;
            do
            {
                int j = (l + h) / 2;
                uint ancestor = key >> (width - j);
                if (!table[j].TryGetValue(ancestor, out node))
                {
                    h = j;
                }
                else
                {
                    l = j;
                }
            }
            while (h - l <= 1);
            return node;
        }

        KeyValuePair<uint, T>? ISortedDictionary<uint, T>.First()
        {
            throw new NotImplementedException();
        }

        KeyValuePair<uint, T>? ISortedDictionary<uint, T>.Last()
        {
            throw new NotImplementedException();
        }

        private LeafNode LowerNode(uint key)
        {
            Node ancestor = Bottom(key);
            if (ancestor == null)
                return null;
            LeafNode leaf = ancestor.right as LeafNode;
            if (leaf != null && leaf.key < key)
                return leaf;
            leaf = ancestor.left as LeafNode;
            if (leaf != null && leaf.key < key)
                return leaf;
            if (ancestor.left != null)
            {
                leaf = ancestor.left.left as LeafNode;
                if (leaf != null && leaf.key < key)
                    return leaf;
            }
            return null;
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

        T IDictionary<uint, T>.this[uint key]
        {
            get
            {
                throw new NotImplementedException();
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
