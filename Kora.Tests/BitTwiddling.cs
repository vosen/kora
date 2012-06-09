using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UAM.Kora;

namespace UAM.Kora.Tests
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
            Assert.AreEqual(1, BitHacks.Power2MSB(a));
            a = 4;
            Assert.AreEqual(2, BitHacks.Power2MSB(a));
            a = 8;
            Assert.AreEqual(3, BitHacks.Power2MSB(a));
            a = 16;
            Assert.AreEqual(4, BitHacks.Power2MSB(a));
            a = 32;
            Assert.AreEqual(5, BitHacks.Power2MSB(a));
            a = 64;
            Assert.AreEqual(6, BitHacks.Power2MSB(a));
            a = 128;
            Assert.AreEqual(7, BitHacks.Power2MSB(a));
        }

        [Test]
        public void MaxValue()
        {
            Assert.AreEqual(1, BitHacks.MaxValue(1));
            Assert.AreEqual(3, BitHacks.MaxValue(2));
            Assert.AreEqual(uint.MaxValue, BitHacks.MaxValue(32));
        }
    }
}
