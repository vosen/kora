using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UAM.Kora
{
    partial class YFastTrie<T>
    {
        class RBUIntNode : RBTree.Node
        {
            internal uint value;

            public RBUIntNode(uint val)
            {
                value = val;
            }

            public override void SwapValue(RBTree.Node other)
            {
                RBUIntNode node = (RBUIntNode)other;
                uint temp = value;
                this.value = node.value;
                node.value = temp;
            }
        }
    }
}
