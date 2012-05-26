using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UAM.Kora;
using YTrie = UAM.Kora.YFastTrie<string>;
using KVP = System.Collections.Generic.KeyValuePair<uint, string>;
using Node = UAM.Kora.YFastTrie<string>.RBUIntNode;

namespace KoraTests
{
    [TestFixture]
    class YFastTrie
    {
        [Test]
        public void RBTreeFromSortedList()
        {
            RBTree empty = YTrie.FromSortedList(new Node[0], 0, 0);
            Assert.DoesNotThrow(() => empty.VerifyInvariants());
            RBTree singleton = YTrie.FromSortedList(new Node[] { new Node(0, "0") }, 0, 0);
            Assert.DoesNotThrow(() => singleton.VerifyInvariants());
            RBTree fullTree = YTrie.FromSortedList(new Node[] { new Node(0, "0"), new Node(1, "1"), new Node(2, "2"), new Node(3, "3"), new Node(4, "4"), new Node(5, "5"), new Node(6, "6") }, 0, 6);
            Assert.DoesNotThrow(() => fullTree.VerifyInvariants());
            RBTree nonFull = YTrie.FromSortedList(new Node[] { new Node(0, "0"), new Node(1, "1"), new Node(2, "2"), new Node(3, "3"), new Node(4, "4"), new Node(5, "5"), new Node(6, "6"), new Node(7, "7") }, 0, 7);
            Assert.DoesNotThrow(() => nonFull.VerifyInvariants());
        }

        [Test]
        public void Creation()
        {
            YTrie trie;
            Assert.DoesNotThrow(() => trie = new YTrie());
        }

        [Test]
        public void GetFromEmpty()
        {
            YTrie trie = new YTrie();
            string temp;
            Assert.IsFalse(trie.TryGetValue(0, out temp));
        }

        [Test]
        public void GetFromNonEmpty()
        {
            YTrie trie = new YTrie();
            trie.Add(845, "845-1");
            Assert.AreEqual("845-1", trie[845]);
            trie[845] = "845-2";
            Assert.AreEqual("845-2", trie[845]);
            trie.Add(815, "815");
            Assert.AreEqual("815", trie[815]);
        }

        [Test]
        public void AddWithSplit()
        {
            YTrie trie = new YTrie();
            trie[0] = "0";
            trie.Add(1, "1");
            trie.Add(2, "2");
            trie.Add(3, "3");
            trie.Add(4, "4");
            trie.Add(5, "5");
            trie.Add(6, "6");
            trie.Add(7, "7");
            trie.Add(8, "8");
            trie.Add(9, "9");
            trie.Add(10, "10");
            trie.Add(11, "11");
            trie.Add(12, "12");
            trie.Add(13, "13");
            trie.Add(14, "14");
            trie.Add(15, "15");
            trie.Add(16, "16");
            trie.Add(17, "17");
            trie.Add(18, "18");
            trie.Add(19, "19");
            trie.Add(20, "20");
            trie.Add(21, "21");
            trie.Add(22, "22");
            trie.Add(23, "23");
            trie.Add(24, "24");
            trie.Add(25, "25");
            trie.Add(26, "26");
            trie.Add(27, "27");
            trie.Add(28, "28");
            trie.Add(29, "29");
            trie.Add(30, "30");
            trie.Add(31, "31");
            trie.Add(32, "32");
            trie.Add(33, "33");
            trie.Add(34, "34");
            trie.Add(35, "35");
            trie.Add(36, "36");
            trie.Add(37, "37");
            trie.Add(38, "38");
            trie.Add(39, "39");
            trie.Add(40, "40");
            trie.Add(41, "41");
            trie.Add(42, "42");
            trie.Add(43, "43");
            trie.Add(44, "44");
            trie.Add(45, "45");
            trie.Add(46, "46");
            trie.Add(47, "47");
            trie.Add(48, "48");
            trie.Add(49, "49");
            trie.Add(50, "50");
            trie.Add(51, "51");
            trie.Add(52, "52");
            trie.Add(53, "53");
            trie.Add(54, "54");
            trie.Add(55, "55");
            trie.Add(56, "56");
            trie.Add(57, "57");
            trie.Add(58, "58");
            trie.Add(59, "59");
            trie.Add(60, "60");
            trie.Add(61, "61");
            trie.Add(62, "62");
            trie.Add(63, "63");
            trie.Add(64, "64");
            Assert.AreEqual("0", trie[0]);
            Assert.AreEqual("1", trie[1]);
            Assert.AreEqual("2", trie[2]);
            Assert.AreEqual("3", trie[3]);
            Assert.AreEqual("4", trie[4]);
            Assert.AreEqual("5", trie[5]);
            Assert.AreEqual("6", trie[6]);
            Assert.AreEqual("7", trie[7]);
            Assert.AreEqual("8", trie[8]);
            Assert.AreEqual("9", trie[9]);
            Assert.AreEqual("10", trie[10]);
            Assert.AreEqual("11", trie[11]);
            Assert.AreEqual("12", trie[12]);
            Assert.AreEqual("13", trie[13]);
            Assert.AreEqual("14", trie[14]);
            Assert.AreEqual("15", trie[15]);
            Assert.AreEqual("16", trie[16]);
            Assert.AreEqual("17", trie[17]);
            Assert.AreEqual("18", trie[18]);
            Assert.AreEqual("19", trie[19]);
            Assert.AreEqual("20", trie[20]);
            Assert.AreEqual("21", trie[21]);
            Assert.AreEqual("22", trie[22]);
            Assert.AreEqual("23", trie[23]);
            Assert.AreEqual("24", trie[24]);
            Assert.AreEqual("25", trie[25]);
            Assert.AreEqual("26", trie[26]);
            Assert.AreEqual("27", trie[27]);
            Assert.AreEqual("28", trie[28]);
            Assert.AreEqual("29", trie[29]);
            Assert.AreEqual("30", trie[30]);
            Assert.AreEqual("31", trie[31]);
            Assert.AreEqual("32", trie[32]);
            Assert.AreEqual("33", trie[33]);
            Assert.AreEqual("34", trie[34]);
            Assert.AreEqual("35", trie[35]);
            Assert.AreEqual("36", trie[36]);
            Assert.AreEqual("37", trie[37]);
            Assert.AreEqual("38", trie[38]);
            Assert.AreEqual("39", trie[39]);
            Assert.AreEqual("40", trie[40]);
            Assert.AreEqual("41", trie[41]);
            Assert.AreEqual("42", trie[42]);
            Assert.AreEqual("43", trie[43]);
            Assert.AreEqual("44", trie[44]);
            Assert.AreEqual("45", trie[45]);
            Assert.AreEqual("46", trie[46]);
            Assert.AreEqual("47", trie[47]);
            Assert.AreEqual("48", trie[48]);
            Assert.AreEqual("49", trie[49]);
            Assert.AreEqual("50", trie[50]);
            Assert.AreEqual("51", trie[51]);
            Assert.AreEqual("52", trie[52]);
            Assert.AreEqual("53", trie[53]);
            Assert.AreEqual("54", trie[54]);
            Assert.AreEqual("55", trie[55]);
            Assert.AreEqual("56", trie[56]);
            Assert.AreEqual("57", trie[57]);
            Assert.AreEqual("58", trie[58]);
            Assert.AreEqual("59", trie[59]);
            Assert.AreEqual("60", trie[60]);
            Assert.AreEqual("61", trie[61]);
            Assert.AreEqual("62", trie[62]);
            Assert.AreEqual("63", trie[63]);
            Assert.AreEqual("64", trie[64]);
            string temp;
            Assert.Throws<KeyNotFoundException>(() => temp = trie[65]);
        }
    }
}
