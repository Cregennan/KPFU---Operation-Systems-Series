using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cregennan.Core
{
    public static class Extensions
    {
        /// <summary>
        /// Преобразует BitArray в IEnumerable по LSB
        /// </summary>
        /// <param name="bits"></param>
        /// <param name="MSB"></param>
        /// <returns></returns>
        public static IEnumerable<byte> ToBytes(this BitArray bits, bool MSB = false)
        {
            int bitCount = 7;
            int outByte = 0;

            foreach (bool bitValue in bits)
            {
                if (bitValue)
                    outByte |= MSB ? 1 << bitCount : 1 << (7 - bitCount);
                if (bitCount == 0)
                {
                    yield return (byte)outByte;
                    bitCount = 8;
                    outByte = 0;
                }
                bitCount--;
            }

            if (bitCount < 7)
                yield return (byte)outByte;
        }

        /// <summary>
        /// Разбивает строку на подстроки заданного размера
        /// </summary>
        /// <param name="str"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IEnumerable<string> Split(this string str, int n)
        {
            if (String.IsNullOrEmpty(str) || n < 1)
            {
                throw new ArgumentException();
            }

            for (int i = 0; i < str.Length; i += n)
            {
                yield return str.Substring(i, Math.Min(n, str.Length - i));
            }
        }

        public static IEnumerable<byte> Indexes(this string s, string alhpabet) => s.Select(y => (byte)alhpabet.IndexOf(y));

        public static IEnumerable<char> FromIndexes(this byte[] a, string alphabet) => a.Select(y => alphabet[y]);

    }
}
