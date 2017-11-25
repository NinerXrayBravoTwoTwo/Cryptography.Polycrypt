/*
 * Copyright (c) 2013-2017 Jillian England
 * GPL 3
 */

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolyCrypt;
using PolyCrypt.DataGen;
using PolyCrypt.GenPoly;

namespace PolyTest
{
    [TestClass]
    public class PolyTest
    {
        private readonly string plainText0 =
                "ITSYBITSYSPIDERWENTUPTHEWATERSPOUTDOWNCAMETHERAINANDWASHEDTHESPIDEROUTOUTCAMETHESUNANDDRIEDUPALLTHERAINSOTHEITSYBITSYSPIDERWHENTUPTHESPOUTAGAIN";

        [TestMethod]
        public void GenVigenere()
        {
            var poly = GenPoly.GenVigenere("n", "abcdefghijklmnopqrstuvwxyz");

            Assert.AreEqual(1, poly.Length);

            Assert.AreEqual(26, poly[0].Length);

            Assert.AreEqual("n", poly[0][0]);
        }

        [TestMethod]
        public void GenRandomAlpha()
        {
            const string alphabet = "AaBbCcDd";
            var poly = GenPoly.GenRandom(string.Empty, alphabet, 1);

            //Assert.AreNotEqual(0, hash);
            Assert.AreEqual(1, poly.Length); // 1 Alphabet
            Assert.AreEqual(alphabet.Length, poly[0].Length); // With 8 characters

            var result = string.Join("", poly[0]);

            Console.WriteLine("{0} {1}", alphabet, result);

            Assert.IsFalse(alphabet.Equals(result));
        }

        [TestMethod]
        public void CryptVigenereInstantiate()
        {
            var password = RandomGen.RName(RandomGen.Next(2, 5), Crypt.Map[0]);
            var test = new Crypt(0, password);

            Assert.IsNotNull(test);

            Assert.AreEqual(password.Length, test.Poly.Length);

            Console.WriteLine(test.ToString());
        }

        [TestMethod]
        public void CryptRandomPolyInstantiate()
        {
            var numPoly = RandomGen.Next(1, 8);

            var test = new Crypt(0, "foGarbee", numPoly);
            Assert.IsNotNull(test);

            Assert.AreEqual(numPoly, test.Poly.Length);

            Console.WriteLine(test.ToString());
        }


        [TestMethod]
        public void CryptRandomPolyVigenere()
        {
            var password = RandomGen.RName(RandomGen.Next(2, 5), Crypt.Map[0]);
            var test = new Crypt(0, password);

            var crypt = test.ToCrypt(plainText0);
            var plain = test.ToPlain(crypt);

            Assert.AreEqual(plainText0, plain);
        }

        [TestMethod]
        public void CryptRandomPolyCipher()
        {
            var numPoly = RandomGen.Next(1, 8);

            var test = new Crypt(0, "foGarbee", numPoly);

            var crypt = test.ToCrypt(plainText0);
            var plain = test.ToPlain(crypt);

            Assert.AreEqual(plainText0, plain);
        }

        [TestMethod]
        public void HashTest()
        {
            var i = 0;
            while ( i++ < 7000)
            {
                // Verify that the hash generated is not zero.
                var password = RandomGen.RName(30);
                var test = GenPoly.GenPasswordHash(password);
                Assert.IsFalse(test == 0);

                // and verify that the hash generated is repeatable.
                Assert.AreEqual(test, GenPoly.GenPasswordHash(password));
            }
        }
    }
}