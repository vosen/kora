using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UAM.Kora;

namespace UAM.KoraTests
{
    [TestFixture]
    class XFastTrie
    {
        [Test]
        public void Creation()
        {
            Assert.DoesNotThrow(() => new XFastTrie<string>());
        }

        [Test]
        public void SimpleAddition()
        {
            XFastTrie<string> trie = null;
            Assert.DoesNotThrow(() => trie = new XFastTrie<string>());
            trie.Add(1, "1");
        }
    }
}
