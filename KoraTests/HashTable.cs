using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UAM.Kora;

namespace UAM.KoraTests
{
    [TestFixture]
    class HashTable
    {
        [Test]
        public void Creation()
        {
            var table = new HashTable<string>();
        }

        [Test]
        public void Adding()
        {
            var table = new HashTable<string>();
            table.Add(1, "1");
            table.Add(2, "2");
            table.Add(3, "3");
            table.Add(4, "4");
            table.Add(5, "5");
            table.Add(6, "6");
            table.Add(7, "7");
            table.Add(8, "8");
            string temp = null;
            Assert.IsTrue(table.TryGetValue(1, ref temp));
            Assert.AreEqual("1", temp);
            Assert.IsTrue(table.TryGetValue(2, ref temp));
            Assert.AreEqual("2", temp);
            Assert.IsTrue(table.TryGetValue(3, ref temp));
            Assert.AreEqual("3", temp);
            Assert.IsTrue(table.TryGetValue(4, ref temp));
            Assert.AreEqual("4", temp);
            Assert.IsTrue(table.TryGetValue(5, ref temp));
            Assert.AreEqual("5", temp);
            Assert.IsTrue(table.TryGetValue(6, ref temp));
            Assert.AreEqual("6", temp);
            Assert.IsTrue(table.TryGetValue(7, ref temp));
            Assert.AreEqual("7", temp);
            Assert.IsTrue(table.TryGetValue(8, ref temp));
            Assert.AreEqual("8", temp);
        }

        [Test]
        public void AddWithCollision()
        {
            var table = new HashTable<string>();
            table.Add(1, "1");
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
            string temp = null;
            Assert.IsTrue(table.TryGetValue(collision, ref temp));
            Assert.AreEqual(collision.ToString(), temp);
        }
    }
}
