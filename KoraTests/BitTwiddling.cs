using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UAM.Kora;

namespace UAM.KoraTests
{
    [TestFixture]
    class BitTwiddling
    {
        [Test]
        public void RoundToPower()
        {
            uint a = 2;
            Assert.AreEqual(2, BitHacks.RoundToPower(a));
            a = 3;
            Assert.AreEqual(4, BitHacks.RoundToPower(a));
            a = 128;
            Assert.AreEqual(128, BitHacks.RoundToPower(a));
            a = 65;
            Assert.AreEqual(128, BitHacks.RoundToPower(a));
            a = 999;
            Assert.AreEqual(1024, BitHacks.RoundToPower(a));
            a = 1;
            Assert.AreEqual(1, BitHacks.RoundToPower(a));
        }

        [Test]
        public void LogCeiling()
        {
            uint a = 2;
            Assert.AreEqual(1, BitHacks.Log2Ceiling(a));
            a = 3;
            Assert.AreEqual(2, BitHacks.Log2Ceiling(a));
            a = 128;
            Assert.AreEqual(7, BitHacks.Log2Ceiling(a));
            a = 65;
            Assert.AreEqual(7, BitHacks.Log2Ceiling(a));
            a = 999;
            Assert.AreEqual(10, BitHacks.Log2Ceiling(a));
            a = 1;
            Assert.AreEqual(0, BitHacks.Log2Ceiling(a));
        }
    }
}
