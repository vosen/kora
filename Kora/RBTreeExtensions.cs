﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UAM.Kora
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

        public static void CopySorted(this RBTree tree, RBTree.Node[] array, int offset)
        {
            InOrderAdd(tree.root, array, offset);
        }

        public static RBTree.Node[] ToSortedArray(this RBTree tree)
        {
            RBTree.Node[] array = new RBTree.Node[tree.Count];
            CopySorted(tree, array, 0);
            return array;
        }

        private static int InOrderAdd(RBTree.Node node, RBTree.Node[] array, int index)
        {
            if (node == null)
                return index;
            index = InOrderAdd(node.left, array, index);
            array[index++] = node;
            return InOrderAdd(node.right, array, index);
        }
    }
}
