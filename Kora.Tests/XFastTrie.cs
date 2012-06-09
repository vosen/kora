using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UAM.Kora;
using KVP = System.Collections.Generic.KeyValuePair<uint, string>;

namespace UAM.Kora.Tests
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
            XFastTrie<string> trie = new XFastTrie<string>();
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

        [Test]
        public void Deletion()
        {
            XFastTrie<string> trie = new XFastTrie<string>();
            string temp;
            trie.Add(0, "0");
            trie.Verify();
            trie.Add(1, "1");
            trie.Verify();
            Assert.IsTrue(trie.Remove(0));
            trie.Verify();
            Assert.IsFalse(trie.TryGetValue(0, out temp));
            Assert.IsTrue(trie.TryGetValue(1, out temp));
            Assert.AreEqual("1", temp);
            Assert.IsTrue(trie.Remove(1));
            trie.Verify();
            trie.Add(110, "110");
            trie.Verify();
            Assert.IsTrue(trie.Remove(110));
            trie.Verify();
            Assert.IsFalse(trie.Remove(110));
            Assert.IsFalse(trie.TryGetValue(110, out temp));
            trie.Add(270, "270");
            trie.Verify();
            trie.Add(182, "182");
            trie.Verify();
            trie.Add(180, "180");
            trie.Verify();
            trie.Add(184, "184");
            trie.Verify();
            trie.Add(40, "40");
            trie.Verify();
            trie.Add(200, "200");
            trie.Verify();
            trie.Add(461, "461");
            trie.Add(158, "158");
            trie.Add(763, "763");
            Assert.IsFalse(trie.Remove(370));
            Assert.IsFalse(trie.Remove(110));
            Assert.IsTrue(trie.Remove(461));
            trie.Verify();
            Assert.IsFalse(trie.TryGetValue(461, out temp));
            Assert.IsFalse(trie.Remove(461));
            Assert.IsTrue(trie.Remove(184));
            trie.Verify();
            Assert.IsFalse(trie.TryGetValue(184, out temp));
            Assert.IsTrue(trie.Remove(763));
            trie.Verify();
            Assert.False(trie.TryGetValue(763, out temp));
        }

        [Test]
        public void AddWithOverwrite()
        {
            XFastTrie<string> trie = new XFastTrie<string>();
            string temp;
            trie.Add(1, "1");
            Assert.Throws<ArgumentException>(() => trie.Add(1, "2"));
            Assert.AreEqual("1", trie[1]);
            Assert.DoesNotThrow(() => trie[1] = "one");
            Assert.AreEqual("one", trie[1]);
        }

        [Test]
        public void LowerHigher()
        {
            XFastTrie<string> trie = new XFastTrie<string>();
            Assert.IsNull(trie.Lower(uint.MaxValue));
            Assert.IsNull(trie.Higher(uint.MinValue));
            trie.Add(699, "699");
            trie.Add(477, "477");
            trie.Add(840, "840");
            trie.Add(324, "324");
            trie.Add(750, "750");
            trie.Add(751, "751");
            trie.Add(563, "563");
            trie.Add(913, "913");
            Assert.IsNull(trie.Lower(324));
            Assert.IsNull(trie.Higher(913));
            Assert.AreEqual(new KVP(563, "563"), trie.Lower(654));
            Assert.AreEqual(new KVP(699, "699"), trie.Higher(654));
            Assert.AreEqual(new KVP(750, "750"), trie.Lower(751));
            Assert.AreEqual(new KVP(751, "751"), trie.Higher(750));
            Assert.AreEqual(new KVP(840, "840"), trie.Higher(751));
        }

        [Test]
        public void FirstLast()
        {
            XFastTrie<string> trie = new XFastTrie<string>();
            Assert.IsNull(trie.First());
            Assert.IsNull(trie.Last());
            trie.Add(100, "100");
            Assert.AreEqual(new KVP(100, "100"), trie.First());
            Assert.AreEqual(new KVP(100, "100"), trie.Last());
            trie.Add(200, "200");
            Assert.AreEqual(new KVP(100, "100"), trie.First());
            Assert.AreEqual(new KVP(200, "200"), trie.Last());
        }
    }
}