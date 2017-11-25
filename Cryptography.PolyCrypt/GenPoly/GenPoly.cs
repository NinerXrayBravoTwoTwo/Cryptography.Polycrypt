/*
 * Copyright (c) 2013-2017 Jillian England
 * GPL 3
 */

using System;
using System.Collections.Generic;
using System.Linq;
using PolyCrypt.DataGen;

namespace PolyCrypt.GenPoly
{
    public static class GenPoly
    {
        #region Generate Vigenere Square

        private static string[] StringToArray(string splitme)
        {
            var result = new string[splitme.Length];

            for (var i = 0; i < splitme.Length; i++)
                result[i] = splitme.Substring(i, 1);

            return result;
        }

        public static string[][] GenVigenere(string password, string alphabet)
        {
            var source = StringToArray(alphabet);

            // the number of alphabets is the length of the password otherwise n is 1 and offset is 3

            int n;
            var hasPswd = false;

            string[] pswdArray;

            if (string.IsNullOrEmpty(password))
            {
                n = 1;
                pswdArray = null;
            }
            else
            {
                // Validate that all letters in password are in the source alphabet.
                pswdArray = StringToArray(password);

                foreach (var item in pswdArray.Where(item => alphabet.IndexOf(item, StringComparison.Ordinal) < 0))
                {
                    Console.WriteLine(
                        $"Password for Vigenere cipher: '{item}' not in '{alphabet}'");
                    return null;
                }
                n = pswdArray.Length;

                hasPswd = true;
            }

            var result = new string[n][];

            for (var i = 0; i < n; i++)
                if (hasPswd)
                {
                    var offset = alphabet.IndexOf(pswdArray[i], StringComparison.Ordinal);
                    result[i] = GenVigenere(offset, source);
                }
                else
                {
                    result[i] = GenVigenere(3, source);
                }

            return result;
        }

        private static string[] GenVigenere(int offset, IReadOnlyList<string> source)
        {
            var result = new string[source.Count];

            for (var i = 0; i < source.Count; i++)
                result[i] = source[(i + offset) % source.Count];

            return result;
        }

        #endregion

        #region Generate Random Poly Alphabet


        public static string[][] GenRandom(string password, string alphabet, int n)
        {
            GenPasswordHash(password);

            var source = StringToArray(alphabet);

            var result = new string[n][];

            for (var i = 0; i < n; i++)
                result[i] = RandomSelect<string>.Group(source);

            return result;
        }

        public static int GenPasswordHash(string password)
        {
            if (string.IsNullOrEmpty(password))
                return RandomGen.Next();

            var hash = Hash.HashInt32(password);

            if (hash == 0 )
                throw new ArithmeticException("Random number initialization failure in password hash.");

            RandomGen.SetSeed(hash);

            return hash;
        }

        #endregion
    }
}