using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UAM.Kora
{
    partial class XFastTrie<T>
    {
        internal class LeafNode : Node
        {
            internal uint key;
            internal T value;
        }
    }
}
