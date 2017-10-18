using System;
using System.Text;

namespace PolyCrypt.DataGen
{
    public static class RandomGen
    {
        private static Random _lcg = new Random();
        private static readonly RandomLong LcgLong = new RandomLong(_lcg.Next());

        #region Random Number Tools (Its better to use one sequence than several, using several will overlap sooner, be less 'random', than just using one.)

        public static void SetSeed(int seed)
        {
            _lcg = new Random(seed);
        }

        public static int Next()
        {
            return _lcg.Next();
        }

        public static int Next(int maxValue)
        {
            return _lcg.Next(maxValue);
        }

        public static int Next(int minValue, int maxValue)
        {
            return _lcg.Next(minValue, maxValue);
        }

        public static long NextLong(long minValue, long maxValue)
        {
            return LcgLong.Next(minValue, maxValue);
        }

        public static long NextLong()
        {
            return LcgLong.Next();
        }

        public static bool NextBool()
        {
            return Next(2) == 1;
        }

        public static double NextDouble()
        {
            return _lcg.NextDouble();
        }

        public static decimal NextDecimal(decimal minValue, decimal maxValue)
        {
            var min = Convert.ToInt32(decimal.Round(minValue*100, 0));
            var max = Convert.ToInt32(decimal.Round(maxValue*100, 0));

            // ReSharper disable PossibleLossOfFraction
            decimal r = Next(min, max)/100; // We intend this.
            // ReSharper restore PossibleLossOfFraction

            return Math.Round(r, 2);
        }

        public static decimal NextDecimal()
        {
            return NextDecimal(Math.Round(decimal.MinValue/100), Math.Round(decimal.MaxValue/100));
        }

        #endregion

        #region Random Name Tools

        /// <summary>
        ///     Generate a random name of length characters.
        /// </summary>
        /// <param name="length">Length of string to generate</param>
        /// <param name="alphaBeta">A string containg the alphabet character you wish to use. i.e. "ABCDEFG12345zyxwv"</param>
        /// <returns>A random string from our character set.</returns>
        public static string RName(int length, string alphaBeta)
        {
            // initialize the rnd with least significant bits of time.

            var sb = new StringBuilder();
            var letters = alphaBeta.ToCharArray();

            var i = length;
            while (i-- > 0)
            {
                var f = _lcg.Next(letters.Length);
                sb.Append(letters[f]);

                // Debug to visually verify that we get a sample of ALL the letters in the string.
                // if (letters.Length <=  f+1) Console.WriteLine("{0} , {1}", letters.Length, f);
                // PsudoCode if after enough repetitions we never access letters[letters.length-1] this logic is in error.
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Generate a random string of length characters.
        /// </summary>
        /// <param name="length">Length of string to generate</param>
        /// <returns>A random character string.</returns>
        public static string RName(int length)
        {
            const string alphaBeta = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz 0123456789_-";
            return RName(length, alphaBeta);
        }

        /// <summary>
        ///     Internal function to generate a random name of n characters.
        ///     Uses a fixed charactor set.
        /// </summary>
        /// <returns>A random string from our character set.</returns>
        public static string RName()
        {
            return RName(20);
        }

        #endregion
    }
}