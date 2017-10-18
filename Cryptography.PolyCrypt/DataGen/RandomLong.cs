using System;

namespace PolyCrypt.DataGen
{
    public sealed class RandomLong
    {
        internal static long Successfulltest = 8423218198450307549;
        internal Random Lcg;

        /// <summary>
        ///     Initialize with a seed equal to 1.
        /// </summary>
        public RandomLong() : this(1)
        {
        }

        /// <summary>
        ///     Initialize with a seed equal to 'seed'
        /// </summary>
        /// <param name="seed"></param>
        public RandomLong(int seed)
        {
            Lcg = new Random(seed);
        }

        /// <summary>
        ///     Get next number in sequence.
        /// </summary>
        /// <returns></returns>
        public long Next()
        {
            var buffer = new byte[8];

            Lcg.NextBytes(buffer);

            var n = 0;
            long result = 0;

            while (n < buffer.Length)
                result = result ^ (long) buffer[n] << n++*8;

            return Math.Abs(result); // Wipe the sign bit.
        }

        /// <summary>
        ///     Return the next sequence item and scale it to be greator than or equal to minValue and less than maxValue
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public long Next(long min, long max)
        {
            var next = Next();

            var range = max - min;

            var scale = (long) Math.Ceiling(next%(decimal) range);

            return scale + min;
        }

        /// <summary>
        ///     Test that math produces the expected result and that seed check is working.
        /// </summary>
        /// <returns></returns>
        public static bool SelfTest()
        {
            // First test the LCG. This loop should
            // exercise any math errors.
            var t = new RandomLong(1);
            long result = 0;

            for (var i = 1; i <= 1000; i++)
                result = t.Next();

            if (result != Successfulltest)
                throw new ArithmeticException($"Expected result '{Successfulltest}' found result '{result}'");

            return true;
        }
    }
}