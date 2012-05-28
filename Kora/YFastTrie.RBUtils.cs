using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UAM.Kora
{
    partial class YFastTrie<T>
    {
        internal class RBUtils
        {
            internal static RBUIntNode HigherNode(RBTree tree, uint key)
            {
                RBUIntNode highParent = null;
                RBUIntNode current = (RBUIntNode)tree.root;
                while (current != null)
                {
                    if (key < current.key)
                    {
                        highParent = current;
                        current = (RBUIntNode)current.left;
                    }
                    else if (key > current.key)
                    {
                        current = (RBUIntNode)current.right;
                    }
                    else
                    {
                        break;
                    }
                }
                // we finished walk on a node with key equal to ours
                if (current != null && current.right != null)
                {
                    return (RBUIntNode)current.right.FirstNode();
                }
                return highParent;
            }

            internal static RBUIntNode LowerNode(RBTree tree, uint key)
            {
                RBUIntNode lowParent = null;
                RBUIntNode current = (RBUIntNode)tree.root;
                while (current != null)
                {
                    if (key < current.key)
                    {
                        current = (RBUIntNode)current.left;
                    }
                    else if (key > current.key)
                    {
                        lowParent = current;
                        current = (RBUIntNode)current.right;
                    }
                    else
                    {
                        break;
                    }
                }
                // we finished walk on a node with key equal to ours
                if (current != null && current.left != null)
                {
                    return (RBUIntNode)current.left.LastNode();
                }
                return lowParent;
            }

            internal static RBTree FromSortedList(RBTree.Node[] list, int start, int stop)
            {
                RBTree tree = new RBTree(new RBUIntNodeHelper());
                int length = stop - start + 1;
                if (start == stop)
                    return tree;
                int maxDepth = BitHacks.Power2MSB(BitHacks.RoundToPower((uint)(length + 1))) - 1;
                tree.root = list[start + (length >> 1)];
                tree.root.IsBlack = true;
                tree.root.Size = (uint)length;
                RBInsertChildren(tree.root, true, 1, maxDepth, list, start, start + (length >> 1) - 1);
                RBInsertChildren(tree.root, false, 1, maxDepth, list, start + (length >> 1) + 1, stop);
                return tree;
            }

            static void RBInsertChildren(RBTree.Node node, bool left, int depth, int totalDepth, RBTree.Node[] list, int start, int stop)
            {
                if (start > stop)
                {
                    if (left)
                        node.left = null;
                    else
                        node.right = null;
                    return;
                }
                int middle = start + ((stop - start) >> 1);
                RBTree.Node current = list[middle];
                current.Size = (uint)(stop - start) + 1;
                current.IsBlack = (((totalDepth - depth) & 1) == 1);
                if (left)
                    node.left = current;
                else
                    node.right = current;
                RBInsertChildren(current, true, depth + 1, totalDepth, list, start, middle - 1);
                RBInsertChildren(current, false, depth + 1, totalDepth, list, middle + 1, stop);
            }
        }
    }
}
