using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UAM.Kora
{
    partial class YFastTrie<T>
    {
        class RBUIntNodeHelper : RBTree.INodeHelper<uint>
        {
            public int Compare(uint key, RBTree.Node node)
            {
                return key.CompareTo(((RBUIntNode)node).value);
            }

            public RBTree.Node CreateNode(uint key)
            {
                return new RBUIntNode(key);
            }
        }
    }
}
