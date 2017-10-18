/*
 * Copyright (c) 2013-2017 Jillian England
 * GPL 3
 */
using System;
using System.Text;

namespace PolyCrypt
{
    public static class CryptPlain
    {
        #region crypt algorithms

        internal static string ToCrypt(string map, string[][] poly, string plain)
        {
            if (poly == null) return string.Empty;
            var result = new StringBuilder();

            for (var i = 0; i < plain.Length; i++)
            {
                var polyindex = i % poly.Length;
                var x = ToCrypt(plain.Substring(i, 1), poly[polyindex], map);
                result.Append(x);
            }

            return result.ToString();
        }

        internal static string ToCrypt(string chr, string[] poly, string map)
        {
            var index = map.IndexOf(chr, StringComparison.Ordinal);

            if (index >= 0 ) return poly[index];

            Console.WriteLine($"Char '{chr}' is not part of valid alphabet:  '{map}'");
            return "@";
        }

        public static string ToPlain(string map, string[][] poly, string cipher)
        {
            var result = new StringBuilder();

            for (var i = 0; i < cipher.Length; i++)
            {
                var polyindex = i % poly.Length;
                var x = ToPlain(cipher.Substring(i, 1), poly[polyindex], map);
                result.Append(x);
            }

            return result.ToString();
        }

        internal static string ToPlain(string chr, string[] poly, string map)
        {
            var index = string.Join("", poly).IndexOf(chr, StringComparison.Ordinal);
            return map.Substring(index, 1);
        }

        #endregion
    }
}