using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UAM.Kora
{
    // TODO: add version increment to all mutating operations and check to enumerator
    public partial class VEBTree<T> : SortedDictionaryBase<T>
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

        public VEBTree(int width)
        {
            if (width > 32 || width < 1)
                throw new ArgumentOutOfRangeException();

            this.width = width;
            if (width > 1)
            {
                int highHalf = width / 2 + (width & 1);
                int lowhHalf = width / 2;
                int halfSize = (int)BitHacks.MaxValue(highHalf) + 1;
                summary = new VEBTree<uint>(highHalf);
                cluster = new VEBTree<T>[halfSize];
                for (int i = 0; i < halfSize; i++)
                    cluster[i] = new VEBTree<T>(lowhHalf);
            }
        }

        internal uint HighBits(uint x)
        {
            int leftShift = 32 - width;
            return (x >> width /2);
        }

        internal uint LowBits(uint x)
        {
            int shift = 32 - (width/2);
            return ((x << shift) >> shift);
        }

        internal uint Index(uint x, uint y)
        {
            return (x << (width/2)) | y;
        }

        private void AddChecked(uint key, T value, bool overwrite)
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

            if (!IsLeaf)
            {
                uint high_x = HighBits(key);
                if (cluster[high_x].minKey == null)
                {
                    summary[high_x] = high_x;
                    uint low_x = LowBits(key);
                    cluster[high_x].EmptyAdd(low_x, value);
                }
                else
                {
                    cluster[high_x].AddChecked(LowBits(key), value, overwrite);
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

        public override void Add(uint key, T value)
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

        public override bool ContainsKey(uint key)
        {
            if (key == minKey || key == maxKey)
                return true;
            else if (IsLeaf)
                return false;
            else
                return cluster[HighBits(key)].ContainsKey(LowBits(key));
        }

        public override bool Remove(uint key)
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
            else if (IsLeaf)
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
                bool result = cluster[HighBits(key)].RemoveCore(LowBits(key));
                if(cluster[HighBits(key)].minKey == null)
                {
                    summary.RemoveCore(HighBits(key));
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
                    maxKey = Index(HighBits(key), cluster[HighBits(key)].maxKey.Value);
                    maxValue = cluster[HighBits(key)].maxValue;
                }
                return result;
            }
            return false;
        }

        public override bool TryGetValue(uint key, out T value)
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
            else if (IsLeaf)
            {
                value = default(T);
                return false;
            }
            else
            {
                return cluster[HighBits(key)].TryGetValue(LowBits(key), out value);
            }
        }

        public override T this[uint key]
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

        public override void Clear()
        {
            count = 0;
            version++;
            minKey = null;
            minValue = default(T);
            maxKey = null;
            maxValue = default(T);
            if (!IsLeaf)
            {
                summary.Clear();
                for (int i = 0; i < cluster.Length; i++)
                    cluster[i].Clear();
            }
        }

        public override int Count
        {
            get { return count; }
        }

        public override IEnumerator<KeyValuePair<uint, T>> GetEnumerator()
        {
            return GetEnumerator(0);
        }

        // TODO: speed-up sparse iteration by checking summary trees
        // TODO: speed up iteration by avoiding recursive GetEnumerator calls - we could cheat by unrolling them, uint tree will only have lg(32)+1 levels.
        private IEnumerator<KeyValuePair<uint, T>> GetEnumerator(uint parent)
        {
            if (minKey != null)
            {
                yield return new KeyValuePair<uint, T>((parent << width) + minKey.Value, minValue);
            }
            if(IsLeaf)
            {
                if(minKey != maxKey)
                    yield return new KeyValuePair<uint, T>((parent << width) + maxKey.Value, maxValue);
            }
            else
            {
                for(uint i = 0; i < cluster.Length; i++)
                {
                    var iter = cluster[i].GetEnumerator((parent << (width/2)) + i);
                    while (iter.MoveNext())
                    {
                        yield return iter.Current;
                    }
                }
            }
        }

        #region ISorted


        public override KeyValuePair<uint, T>? First()
        {
            if (minKey == null)
                return null;
            return new KeyValuePair<uint, T>(minKey.Value, minValue);
        }

        public override KeyValuePair<uint, T>? Last()
        {
            if (maxKey == null)
                return null;
            return new KeyValuePair<uint, T>(maxKey.Value, maxValue);
        }

        public override KeyValuePair<uint, T>? Lower(uint key)
        {
            if (IsLeaf)
            {
                if (key == 1 && minKey == 0)
                    return new KeyValuePair<uint, T>(0, minValue);
                else
                    return null;
            }
            else if (maxKey != null && key > maxKey.Value)
            {
                return new KeyValuePair<uint, T>(maxKey.Value, maxValue);
            }
            else
            {
                uint highBits = HighBits(key);
                uint? minLow = cluster[HighBits(key)].minKey;
                if (minLow != null && LowBits(key) > minLow.Value)
                {
                    var offset = cluster[highBits].Lower(LowBits(key));
                    uint returnKey = Index(highBits, offset.Value.Key);
                    return new KeyValuePair<uint, T>(returnKey, offset.Value.Value);
                }
                else
                {
                    var predCluster = summary.Lower(HighBits(key));
                    if (predCluster == null)
                    {
                        if (minKey != null && key > minKey.Value)
                            return new KeyValuePair<uint, T>(minKey.Value, minValue);
                        else
                            return null;
                    }
                    else
                    {
                        var offset = cluster[predCluster.Value.Key].maxKey;
                        return new KeyValuePair<uint, T>(Index(predCluster.Value.Key, offset.Value), cluster[predCluster.Value.Key].maxValue);
                    }
                }
            }
        }

        public override KeyValuePair<uint, T>? Higher(uint key)
        {
            if (IsLeaf)
            {
                if (key == 0 && maxKey == 1)
                    return new KeyValuePair<uint, T>(1, maxValue);
                else
                    return null;
            }
            else if (minKey != null && key < minKey.Value)
            {
                return new KeyValuePair<uint, T>(minKey.Value, minValue);
            }
            else
            {
                uint highBits =  HighBits(key);
                uint? maxLow = cluster[highBits].maxKey;
                if (maxLow != null && LowBits(key) < maxLow.Value)
                {
                    var offset = cluster[highBits].Higher(LowBits(key));
                    uint returnKey = Index(highBits, offset.Value.Key);
                    return new KeyValuePair<uint, T>(returnKey, offset.Value.Value);
                }
                else
                {
                    var succCluster = summary.Higher(HighBits(key));
                    if (succCluster == null)
                    {
                        return null;
                    }
                    else
                    {
                        var offset = cluster[succCluster.Value.Key].minKey;
                        return new KeyValuePair<uint, T>(Index(succCluster.Value.Key, offset.Value), cluster[succCluster.Value.Key].minValue);
                    }
                }
            }

        }

        private bool IsLeaf
        {
            get{ return width == 1; }
        }

        #endregion
    }
}
