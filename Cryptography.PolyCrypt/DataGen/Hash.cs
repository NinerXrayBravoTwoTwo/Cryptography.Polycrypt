using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PolyCrypt.DataGen
{
    public static class Hash
    {
        public static byte[] HashBytes(string value)
        {
            var hash = new SHA256CryptoServiceProvider();
            return hash.ComputeHash(ToBytes(value));
        }

        /// <summary>
        ///     String to Byte []. Each character in the string is turned into two bytes.
        /// </summary>
        /// <param name="unicode"></param>
        /// <returns></returns>
        private static byte[] ToBytes(string unicode)
        {
            return Encoding.Unicode.GetBytes(unicode);
        }

        public static Guid HashGuid(string value)
        {
            var buffer = HashBytes(value);
            Array.Resize(ref buffer, 16);
            return new Guid(buffer);
        }

        /// <summary>
        ///     Compute an Int64  hash value based on data submitted to this via Add. It is possible to generate a 128 bit hash
        ///     value.
        /// </summary>
        /// <returns></returns>
        public static long HashInt64(string value)
        {
            var hash = HashBytes(value);

            byte[] toInt = {hash[0], hash[1], hash[2], hash[3], hash[4], hash[5], hash[6], hash[7]};

            return ToInt64(toInt);
        }

        /// <summary>
        ///     Convert an array of bytes to an int64. ToInt64( ToByte(i) ) == i
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static long ToInt64(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentException("Can not convert null byte array");

            if (buffer.Length > 8)
                throw new ArgumentException("Can not convert more than 8 bytes into an int64.");

            var n = 0;
            long result = 0;

            while (n < buffer.Length)
                result = result ^ (long) buffer[n] << n++*8;

            return result;
        }

        /// <summary>
        ///     Compute an Int64  hash value based on data submitted to this via Add. It is possible to generate a 128 bit hash
        ///     value.
        /// </summary>
        /// <returns></returns>
        public static int HashInt32(string value)
        {
            var hash = HashBytes(value);

            byte[] toInt = {hash[0], hash[1], hash[2], hash[3]};

            return ToInt32(toInt);
        }

        /// <summary>
        ///     Convert 4 bytes back into an int32.  ToInt32(ToByte(i)) == i
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static int ToInt32(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentException("Can not convert null byte array");
            if (buffer.Length > 4)
                throw new ArgumentException("Can not convert more than 4 bytes into an int32.");

            var n = 0;
            var result = 0;

            while (n < buffer.Length)
                result = result ^ buffer[n] << n++*8;

            return result;
        }

        /// <summary>
        ///     Convert each byte in byte array to a string characer seperated by a space.
        /// </summary>
        /// <param name="value">array of bytes</param>
        /// <returns>string</returns>
        public static string FormatToString(IEnumerable<byte> value)
        {
            return string.Join(" ", value.Select(b => b.ToString()).ToArray());
        }

        /// <summary>
        ///     convert an int32 into array of four bytes.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToBytes(int value)
        {
            byte[] result =
            {
                (byte) (value & 0xff)
                , (byte) (value >> 8 & 0xff)
                , (byte) (value >> 16 & 0xff)
                , (byte) (value >> 24 & 0xff)
            };

            return result;
        }

        public static byte[] ToBytes(long value)
        {
            // A long is 8 bytes.
            byte[] result =
            {
                (byte) (value & 0xff)
                , (byte) (value >> 8 & 0xff)
                , (byte) (value >> 16 & 0xff)
                , (byte) (value >> 24 & 0xff)
                , (byte) (value >> 32 & 0xff)
                , (byte) (value >> 40 & 0xff)
                , (byte) (value >> 48 & 0xff)
                , (byte) (value >> 56 & 0xff)
            };

            return result;
        }
    }
}