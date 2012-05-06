using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UAM.Kora
{
    public partial class VEBTree<T> : IDictionary<uint, T>
    {
        private VEBTree<T>[] cluster;
        private VEBTree<uint> summary;
        private uint? minKey;
        private T minValue;
        private uint? maxKey;
        private T maxValue;
        private int width;
        private int count;
        private int version;

        public VEBTree()
            : this(32)
        { }

        internal VEBTree(int width)
        {
            if (width > 32 || width < 1)
                throw new ArgumentOutOfRangeException();

            this.width = width;
            if (width > 1)
            {
                int halfWidth = width / 2;
                int halfSize = (int)(uint.MaxValue >> (32 - (width / 2))) + 1;
                summary = new VEBTree<uint>(halfWidth);
                cluster = new VEBTree<T>[halfSize];
                for (int i = 0; i < halfSize; i++)
                    cluster[i] = new VEBTree<T>(halfWidth);
            }
        }

        private uint High(uint x)
        {
            if (width < 2)
                throw new ArgumentException();

            int leftShift = 32 - width;
            return (x << leftShift) >>(leftShift + width / 2);
        }

        private uint Low(uint x)
        {
            if (width < 2)
                throw new ArgumentException();


            int shift = 32 - (width/2);
            return ((x << shift) >> shift);
        }

        private uint Index(uint x, uint y)
        {
            if (width < 2)
                throw new ArgumentException();

            return (x << (width/2)) + y;
        }

        public void AddChecked(uint key, T value, bool overwrite)
        {

            if (key >= (1 << width))
                throw new ArgumentOutOfRangeException();

            if ((key == minKey || key == maxKey) && !overwrite)
                throw new ArgumentException();

            if (key != minKey && key != maxKey)
                count++;

            if (minKey == null)
            {
                EmptyAdd(key, value);
                return;
            }

            // I use <= nistead of < to indicate case when we want
            // to set new value associated with key already in the set
            if (key <= minKey)
            {
                uint tempKey = key;
                T tempValue = value;
                key = minKey.Value;
                value = minValue;
                minKey = tempKey;
                minValue = tempValue;
            }

            if (width > 1)
            {
                uint high_x = High(key);
                if (cluster[high_x].minKey == null)
                {
                    summary[high_x] = high_x;
                    uint low_x = Low(key);
                    cluster[high_x].EmptyAdd(low_x, value);
                }
                else
                {
                    cluster[high_x].AddChecked(Low(key), value, overwrite);
                }
            }

            // I use >= nistead of > to indicate case when we want
            // to set new value associated with key already in the set
            if (key >= maxKey)
            {
                maxKey = key;
                maxValue = value;
            }
        }

        public void Add(uint key, T value)
        {
            AddChecked(key, value, false);
        }

        private void EmptyAdd(uint key, T value)
        {
            minKey = key;
            minValue = value;
            maxKey = key;
            maxValue = value;
        }

        public bool ContainsKey(uint key)
        {
            if (key == minKey || key == maxKey)
                return true;
            else if (width == 1)
                return false;
            else
                return cluster[High(key)].ContainsKey(Low(key));
        }

        public bool Remove(uint key)
        {
            if (RemoveCore(key))
            {
                count--;
                return true;
            }
            return false;
        }

        private bool RemoveCore(uint key)
        {
            if (minKey == maxKey)
            {
                if (minKey == key)
                {
                    minKey = null;
                    minValue = default(T);
                    maxKey = null;
                    maxValue = default(T);
                    return true;
                }
            }
            // minkey and maxkey are different and we are within leaf
            else if (width == 1)
            {
                if (key == 0)
                {
                    minKey = 1;
                    minValue = maxValue;
                    maxValue = default(T);
                }
                else
                {
                    maxKey = 0;
                    maxValue = minValue;
                    minValue = default(T);
                }
                return true;
            }
            // minkey and maxkey are different and we are not inside leaf
            else
            {
                if (minKey == key)
                {
                    uint firstCluster = summary.minKey.Value;
                    key = Index(firstCluster, cluster[firstCluster].minKey.Value);
                    minKey = key;
                    // update the value
                    minValue = cluster[firstCluster].minValue;
                }
                bool result = cluster[High(key)].RemoveCore(Low(key));
                if(cluster[High(key)].minKey == null)
                {
                    summary.RemoveCore(High(key));
                    if(key == maxKey)
                    {
                        uint? summaryMax = summary.maxKey;
                        if (summaryMax == null)
                        {
                            maxKey = minKey;
                            maxValue = minValue;
                        }
                        else
                        {
                            maxKey = Index(summaryMax.Value, cluster[summaryMax.Value].maxKey.Value);
                            maxValue = cluster[summaryMax.Value].maxValue;
                        }
                    }
                }
                else if (key == maxKey)
                {
                    maxKey = Index(High(key), cluster[High(key)].maxKey.Value);
                    maxValue = cluster[High(key)].maxValue;
                }
                return result;
            }
            return false;
        }

        public bool TryGetValue(uint key, out T value)
        {
            if (key == minKey)
            {
                value = minValue;
                return true;
            }
            else if (key == maxKey)
            {
                value = maxValue;
                return true;
            }
            else if (width == 1)
            {
                value = default(T);
                return false;
            }
            else
            {
                return cluster[High(key)].TryGetValue(Low(key), out value);
            }
        }

        public ICollection<uint> Keys
        {
            get { throw new NotImplementedException(); }
        }

        public ICollection<T> Values
        {
            get { throw new NotImplementedException(); }
        }

        public T this[uint key]
        {
            get
            {
                T value;
                if (TryGetValue(key, out value))
                    return value;
                else
                    throw new KeyNotFoundException();
            }
            set
            {
                AddChecked(key, value, true);
            }
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }


        IEnumerator<KeyValuePair<uint, T>> IEnumerable<KeyValuePair<uint, T>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #region implicits

        void ICollection<KeyValuePair<uint, T>>.Add(KeyValuePair<uint, T> item)
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

        bool ICollection<KeyValuePair<uint, T>>.Remove(KeyValuePair<uint, T> item)
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((ICollection<KeyValuePair<uint, T>>)this).GetEnumerator();
        }

        #endregion
    }
}
