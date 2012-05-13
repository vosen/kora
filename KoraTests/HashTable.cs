using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UAM.Kora;

namespace KoraTests
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
            //table.Add(8, "8");
            string temp = null;
            Assert.IsTrue(table.TryGetValue(1, ref temp));
            Assert.AreEqual("1", temp);
            Assert.IsTrue(table.TryGetValue(2, ref temp));
            Assert.AreEqual("2", temp);
            Assert.IsTrue(table.TryGetValue(3, ref temp));
            Assert.AreEqual("3", temp);
        }
    }
}
