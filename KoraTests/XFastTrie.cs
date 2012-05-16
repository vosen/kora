﻿using System;
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

        [Test]
        public void AddMultiple()
        {
            XFastTrie<string> trie = null;
            Assert.DoesNotThrow(() => trie = new XFastTrie<string>());
            trie.Add(1, "1");
            trie.Add(2, "2");
            trie.Add(31, "31");
            trie.Add(3, "3");
            trie.Add(4, "4");
            trie.Add(5, "5");
            trie.Add(6, "6");
            trie.Add(27, "27");
            trie.Add(21, "21");
            trie.Add(19, "19");
            Assert.AreEqual(trie[1], "1");
            Assert.AreEqual(trie[2], "2");
            Assert.AreEqual(trie[31], "31");
            Assert.AreEqual(trie[3], "3");
            Assert.AreEqual(trie[4], "4");
            Assert.AreEqual(trie[5], "5");
            Assert.AreEqual(trie[6], "6");
            Assert.AreEqual(trie[27], "27");
            Assert.AreEqual(trie[21], "21");
            Assert.AreEqual(trie[19], "19");
        }
    }
}