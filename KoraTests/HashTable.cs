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
            Assert.AreEqual(0, table.Count);
        }
    }
}
