using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolyCrypt;
using PolyCrypt.DataGen;

namespace PolyTest
{
    [TestClass]
    public class HashTest
    {
        [TestMethod]
        public void ToInt64Bytes()
        {
            var buffer = Hash.ToBytes(long.MaxValue);
            var xform = Hash.ToInt64(buffer);

            Assert.AreEqual(long.MaxValue, xform);
        }

        [TestMethod]
        public void ToInt32Bytes()
        {
            var buffer = Hash.ToBytes(int.MaxValue);
            var xform = Hash.ToInt32(buffer);

            Assert.AreEqual(int.MaxValue, xform);
        }

        [TestMethod]
        public void HashGuidTest()
        {
            var randomString = RandomGen.RName();
            var buffer = Hash.HashBytes(randomString);

            Console.WriteLine(buffer.Length);

            var target = Hash.HashGuid(randomString);

            Console.WriteLine(target.ToString());

            Assert.AreNotEqual(Guid.Empty, target);
        }

        [TestMethod]
        public void HashInt32()
        {
            var value = Hash.HashInt32(RandomGen.RName());

            Assert.AreNotEqual(0, value);
        }

        [TestMethod]
        public void HashInt64()
        {
            var value = Hash.HashInt64(RandomGen.RName());

            Assert.AreNotEqual(0, value);
        }

        [TestMethod]
        public void ToBytesString()
        {
        }
    }
}