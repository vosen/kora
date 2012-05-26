using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UAM.Kora;
using YTrie = UAM.Kora.YFastTrie<string>;

namespace KoraTests
{
    [TestFixture]
    class YFastTrie
    {
        [Test]
        public void RBTreeFromSortedList()
        {
            RBTree empty = YTrie.FromSortedList(new uint[0]);
            Assert.DoesNotThrow(() => empty.VerifyInvariants());
            RBTree singleton = YTrie.FromSortedList(new uint[] {0});
            Assert.DoesNotThrow(() => singleton.VerifyInvariants());
            RBTree fullTree = YTrie.FromSortedList(new uint[] { 0,1,2,3,4,5,6 });
            Assert.DoesNotThrow(() => fullTree.VerifyInvariants());
            RBTree nonFull = YTrie.FromSortedList(new uint[] { 0,1,2,3,4,5,6,7 });
            Assert.DoesNotThrow(() => nonFull.VerifyInvariants());
        }
    }
}
