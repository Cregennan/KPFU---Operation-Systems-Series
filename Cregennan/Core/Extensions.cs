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
    }
}
