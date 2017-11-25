/*/*
 * Copyright (c) 2013-2017 Jillian England
 * GPL 3
 */

using System.Collections.Generic;
using System.Text;

namespace PolyCrypt
{
    public enum PolyCipherChoice
    {
        Vigenere, // https://en.wikipedia.org/wiki/Vigen%C3%A8re_cipher
        Random, // ... Poly unknown alphabet
        Autokey //https://en.wikipedia.org/wiki/Autokey_cipher
    }

    public class Crypt
    {
        protected internal static readonly string[] Map =
        {
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
            , "abcdefghijklmnopqrstuvwxyz"
            , "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
            , "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"
            , "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=" // base 64 encoding set.  
            , "0123456789" // number
            , "0123456789ABCDEF" // hex
            // You could base64 your code then run it through Vig/Rand/AutoK with poly alphabets.  Not suitable for hand encryption.
            // QV image generation.  Take a message, Autokey encrypt it, generate a QV bargraph and publish it.
        };

        protected internal readonly string[][] Poly;

        /// <summary>
        ///     Vigenere Cipher with the number of alphabets defined in your password length
        /// </summary>
        /// <param name="defaultAlphabet"></param>
        /// <param name="password"></param>
        public Crypt(int defaultAlphabet, string password)
        {
            Alphabet = Map[defaultAlphabet];

            Poly = GenPoly.GenPoly.GenVigenere(password, Alphabet);
        }

        /// <summary>
        ///     Random poly alpabet cipher with the number of alpabets specified and the random sequence created by a hash of your
        ///     password.
        /// </summary>
        /// <param name="defaultAlphabet"></param>
        /// <param name="password"></param>
        /// <param name="numberAlphabets"></param>
        public Crypt(int defaultAlphabet, string password, int numberAlphabets)
        {
            Alphabet = Map[defaultAlphabet];

            Poly = GenPoly.GenPoly.GenRandom(password, Alphabet, numberAlphabets);
            //Console.WriteLine("hash={0}", hash); // TODO:  Isn't this a pickle? (yes you will have to think to figure this out)
        }

        internal string Alphabet { get; set; }

        /// <summary>
        ///     write result in groups of x characters
        /// </summary>
        /// <param name="textToGroup"></param>
        /// <param name="charsPerGroup"></param>
        /// <returns></returns>
        public static string GroupsOf(string textToGroup, int charsPerGroup)
        {
            var result = new List<string>();

            var index = 0;
            while (index + charsPerGroup < textToGroup.Length)
            {
                result.Add(textToGroup.Substring(index, charsPerGroup));
                index += charsPerGroup;
            }

            if (index < textToGroup.Length)
                result.Add(textToGroup.Substring(index, textToGroup.Length - index));

            return string.Join(" ", result.ToArray());
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            if (Poly == null) return string.Empty;

            result.Append($"Alphabet:\n{Alphabet}\n");
            foreach (var item in Poly)
            {
                result.Append(string.Join("", item));
                result.Append("\n");
            }

            return result.ToString();
        }

        public string ToCrypt(string plain)
        {
            return CryptPlain.ToCrypt(Alphabet, Poly, plain);
        }

        public string ToPlain(string crypt)
        {
            return CryptPlain.ToPlain(Alphabet, Poly, crypt);
        }
    }
}