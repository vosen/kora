using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UAM.Kora
{
    partial class YFastTrie<T>
    {
        internal class Node
        {
            internal static readonly RBUIntNodeHelper Helper = new RBUIntNodeHelper();
            internal RBTree tree;

            public Node(uint key, T value)
            {
                tree = new RBTree(Helper);
                tree.Intern(key, new RBUIntNode(key, value));
            }
        }
    }
}
