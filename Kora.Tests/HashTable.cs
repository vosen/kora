using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UAM.Kora;

namespace UAM.Kora.Tests
{
    [TestFixture]
    class HashTable
    {
        private static void CheckInnerTables(HashTable<string> table)
        {
            foreach (var inner in table.inner)
            {
                foreach (var pair in inner.table)
                {
                    if (pair != null)
                        Assert.AreNotEqual(null, pair.Value);
                }
            }
        }

        [Test]
        public void Creation()
        {
            var table = new HashTable<string>();
            CheckInnerTables(table);
        }

        [Test]
        public void MultipleSameAdd()
        {
            var table = new HashTable<string>();
            table.Add(0, "0");
            for (int i = 0; i < 7; i++)
                Assert.Throws<ArgumentException>(() => table.Add(0, "0"));
            Assert.AreEqual(1, table.Count);
        }

        [Test]
        public void Adding()
        {
            var table = new HashTable<string>();
            table.Add(1, "1");
            CheckInnerTables(table);
            table.Add(2, "2");
            CheckInnerTables(table);
            table.Add(3, "3");
            CheckInnerTables(table);
            table.Add(4, "4");
            CheckInnerTables(table);
            table.Add(5, "5");
            CheckInnerTables(table);
            table.Add(6, "6");
            CheckInnerTables(table);
            table.Add(7, "7");
            CheckInnerTables(table);
            table.Add(8, "8");
            CheckInnerTables(table);
            string temp = null;
            Assert.IsTrue(table.TryGetValue(1, out temp));
            Assert.AreEqual("1", temp);
            Assert.IsTrue(table.TryGetValue(2, out temp));
            Assert.AreEqual("2", temp);
            Assert.IsTrue(table.TryGetValue(3, out temp));
            Assert.AreEqual("3", temp);
            Assert.IsTrue(table.TryGetValue(4, out temp));
            Assert.AreEqual("4", temp);
            Assert.IsTrue(table.TryGetValue(5, out temp));
            Assert.AreEqual("5", temp);
            Assert.IsTrue(table.TryGetValue(6, out temp));
            Assert.AreEqual("6", temp);
            Assert.IsTrue(table.TryGetValue(7, out temp));
            Assert.AreEqual("7", temp);
            Assert.IsTrue(table.TryGetValue(8, out temp));
            Assert.AreEqual("8", temp);
        }

#if DEBUG
        [Test]
        public void AddWithCollision()
        {
            var table = new HashTable<string>();
            table.Add(1, "1");
            CheckInnerTables(table);
            uint firstHash = table.GetHash(1);
            uint collision = 2;
            while(!(table.GetHash(collision) == firstHash))
            {
                collision++;
            }
            // HACK WARNING: I create collision instead of generating it
            uint secondHash = table.inner[firstHash].GetHash(collision);
            table.inner[firstHash].a = 0;
            table.inner[firstHash].b = secondHash;
            table.Add(collision, collision.ToString());
            CheckInnerTables(table);
            string temp = null;
            Assert.IsTrue(table.TryGetValue(collision, out temp));
            Assert.AreEqual(collision.ToString(), temp);
        }
#endif

        [Test]
        public void Delete()
        {
            uint size = 128;
            string temp = null;
            var table = new HashTable<string>();
            for (uint i = 0; i < size; i++)
            {
                table.Add(i, i.ToString());
                CheckInnerTables(table);
            }
            for (uint i = 0; i < size; i++)
            {
                Assert.IsTrue(table.TryGetValue(i, out temp));
                Assert.AreEqual(i.ToString(), temp);
            }
            for (uint i = 0; i < size; i++)
            {
                table.Remove(i);
                CheckInnerTables(table);
            }
            for (uint i = 0; i < size; i++)
            {
                Assert.IsFalse(table.TryGetValue(i, out temp));
            }
        }
    }
}
