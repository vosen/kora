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
            table.Add(2, "1");
            table.Add(3, "1");
        }
    }
}
