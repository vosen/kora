using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UAM.Kora
{
    partial class XFastTrie<T>
    {
        internal class Node
        {
            internal Node left;
            internal Node right;

            internal Node Left
            {
                get { return left; }
            }

            internal Node Right
            {
                get { return right; }
            }

            internal LeafNode Jump
            {
                get
                {
                    LeafNode jump = left as LeafNode;
                    if (jump != null)
                        return jump;
                    jump = right as LeafNode;
                    return jump;
                }
            }
        }
    }
}
