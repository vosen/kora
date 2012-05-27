using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UAM.Kora
{
    partial class YFastTrie<T>
    {
        internal class RBUIntNode : RBTree.Node
        {
            internal uint key;
            internal T value;

            internal RBUIntNode(KeyValuePair<uint, T> pair)
                : this(pair.Key, pair.Value)
            {}

            internal RBUIntNode(uint key)
            {
                this.key = key;
            }

            internal RBUIntNode(uint key, T value)
            {
                this.key = key;
                this.value = value;
            }

            public override void SwapValue(RBTree.Node other)
            {
                RBUIntNode node = (RBUIntNode)other;
                uint tempKey = key;
                this.key = node.key;
                node.key = tempKey;
                T tempValue = this.value;
                this.value = node.value;
                node.value = tempValue;
            }

            public override bool Equals(object obj)
            {
                RBUIntNode node = obj as RBUIntNode;
                return (node != null) && (node.key == key) && (Object.Equals(node.value, value));
            }

            public override int GetHashCode()
            {
                return key.GetHashCode() ^ (value == null ? int.MaxValue : value.GetHashCode());
            }
        }
    }
}
