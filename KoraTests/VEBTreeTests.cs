using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UAM.Kora;

namespace UAM.KoraTests
{
    [TestFixture]
    public class VEBTree
    {
        private static void Ignore<T>(T arg) { }

        private static void CorrectnessCheck(VEBTree<string> tree, IEnumerable<uint> checkSet)
        {
            foreach(uint val in checkSet)
                Assert.AreEqual(val.ToString(), tree[val]);
            Assert.AreEqual(checkSet.Count(), tree.Count);
        }

        [Test]
        public void EmptyCreation()
        {
            var tree = new VEBTree<string>(8);
            for (uint i = 0; i < Math.Pow(2, 8); i++)
                Assert.Throws<KeyNotFoundException>(() => Ignore(tree[i]) );
        }

        [Test]
        public void IncorrectVEBWidth()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new VEBTree<object>(64));
        }

        [Test]
        public void SimpleAdd()
        {
            var tree = new VEBTree<string>(4);
            tree.Add(13, "test1");
            tree.Add(8, "test2");
            tree.Add(10, "test3");
            tree.Add(15, "test4");
            Assert.AreEqual("test1", tree[13]);
            Assert.AreEqual("test2", tree[8]);
            Assert.AreEqual("test3", tree[10]);
            Assert.AreEqual("test4", tree[15]);
        }

        [Test]
        public void IncorrectAddOrSet()
        {
            var tree = new VEBTree<string>(4);
            tree.Add(15, "t1");
            Assert.Throws<ArgumentOutOfRangeException>(() => tree.Add(16, "test"));
            tree[0] = "t2";
            Assert.Throws<ArgumentOutOfRangeException>(() => tree[20] = "test2");
        }

        [Test]
        public void OverlappingAdd()
        {
            var tree = new VEBTree<string>(4);
            tree.Add(13, "test1");
            Assert.Throws<ArgumentException>(() => tree.Add(13, "test2"));
        }

        [Test]
        public void OverlappingItemSet()
        {
            var tree = new VEBTree<string>(4);
            tree[13] = "test1";
            tree[13] = "test2";
            Assert.AreEqual("test2", tree[13]);
        }

        [Test]
        public void SmallRemove()
        {
            var tree = new VEBTree<string>(4);
            tree.Add(5, "asd");
            Assert.IsTrue(tree.Remove(5));
            Assert.IsFalse(tree.Remove(0));
            Assert.Throws<KeyNotFoundException>(() => Ignore(tree[5]));
        }

        [Test]
        public void LargeRemove()
        {
            var tree = new VEBTree<string>(4);
            tree.Add(1, "1");
            tree.Add(8, "8");
            tree.Add(2, "2");
            Assert.IsTrue(tree.Remove(2));
            CorrectnessCheck(tree, new uint[] {1, 8});
            tree.Add(0, "0");
            tree.Add(15, "15");
            tree.Add(12, "12");
            tree.Add(7, "7");
            tree.Add(9, "9");
            tree.Add(10, "10");
            tree.Add(11, "11");
            Assert.IsTrue(tree.Remove(15));
            // start deletion correctness check
            Assert.AreEqual("1", tree[1]);
            CorrectnessCheck(tree, new uint[] { 1, 8, 0, 12, 7, 9, 10, 11 });
            // end deletion correctness check
            Assert.IsTrue(tree.Remove(8));
            CorrectnessCheck(tree, new uint[] { 1, 0, 12, 7, 9, 10, 11 });
            Assert.IsTrue(tree.Remove(0));
            CorrectnessCheck(tree, new uint[] { 1, 12, 7, 9, 10, 11 });
            Assert.IsFalse(tree.Remove(8));
            Assert.IsTrue(tree.Remove(11));
            CorrectnessCheck(tree, new uint[] { 1, 12, 7, 9, 10});
            Assert.IsTrue(tree.Remove(10));
            CorrectnessCheck(tree, new uint[] { 1, 12, 7, 9 });
            Assert.IsTrue(tree.Remove(9));
            CorrectnessCheck(tree, new uint[] { 1, 12, 7 });
            Assert.IsTrue(tree.Remove(12));
            CorrectnessCheck(tree, new uint[] { 1, 7 });
            Assert.Throws<KeyNotFoundException>(() => Ignore(tree[15]));
        }

        [Test]
        public void ContainsKey()
        {
            var tree = new VEBTree<string>(2);
            tree.Add(0, "0");
            tree.Add(1, "1");
            tree.Add(2, "2");
            tree.Add(3, "3");
            Assert.IsTrue(tree.ContainsKey(0));
            Assert.IsTrue(tree.ContainsKey(1));
            Assert.IsTrue(tree.ContainsKey(2));
            Assert.IsTrue(tree.ContainsKey(3));
            tree = new VEBTree<string>(2);
            Assert.IsFalse(tree.ContainsKey(0));
            Assert.IsFalse(tree.ContainsKey(1));
            Assert.IsFalse(tree.ContainsKey(2));
            Assert.IsFalse(tree.ContainsKey(3));
        }


        [Test]
        public void Count()
        {
            var tree = new VEBTree<string>(4);
            tree.Add(1, "1");
            tree.Add(8, "8");
            tree.Add(2, "2");
            Assert.IsTrue(tree.Remove(2));
            tree.Add(0, "0");
            tree.Add(15, "15");
            Assert.IsTrue(tree.Remove(0));
            tree.Add(12, "12");
            tree.Add(7, "7");
            Assert.IsTrue(tree.Remove(8));
            Assert.IsTrue(tree.Remove(12));
            tree.Add(9, "9");
            Assert.AreEqual(4, tree.Count);
        }

    }
}
