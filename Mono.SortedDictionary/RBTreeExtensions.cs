using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mono.Collections.Generic
{
    internal static class RBTreeExtensions
    {
        public static RBTree.Node FirstNode(this RBTree tree)
        {
            return FirstNode(tree.root);
        }

        public static RBTree.Node FirstNode(this RBTree.Node node)
        {
            if (node == null)
                return null;
            while (node.left != null)
                node = node.left;
            return node;
        }

        public static RBTree.Node LastNode(this RBTree.Node node)
        {
            if (node == null)
                return null;
            while (node.right != null)
                node = node.right;
            return node;
        }

        public static RBTree.Node LastNode(this RBTree tree)
        {
            return LastNode(tree.root);
        }
    }
}
