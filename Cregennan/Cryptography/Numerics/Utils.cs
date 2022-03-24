using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Cregennan.Cryptography.Numerics
{
    public class Utils
    {
        /// <summary>
        /// Криптографически стойкий генератор случайных чисел
        /// </summary>
        public static RandomNumberGenerator generator = RandomNumberGenerator.Create();

        public static readonly UInt64 Minimum64 = 0x8000000000000000;
        public static readonly UInt64 Maximum64 = 0xffffffffffffffff;
        public static readonly UInt32 Minimum32 = 0x80000000;
        public static readonly UInt32 Maximum32 = 0xffffffff;
        

        /// <summary>
        /// Получает криптографически стойкое случайное положительное число в заданном диапазоне
        /// </summary>
        /// <param name="maximum">Положительное число</param>
        /// <param name="minimum"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static BigInteger GetRandomInRange(BigInteger minimum, BigInteger maximum)
        {
            if (maximum < minimum)
            {
                throw new ArgumentException("Максимальный предел должен быть не меньше минимального");
            }
            if (maximum < 0 || minimum < 0)
            {
                throw new ArgumentOutOfRangeException("Пределы должны быть положительными");
            }
            
            BigInteger random;
            byte[] temp = maximum.ToByteArray();
            do
            {
                generator.GetBytes(temp);
                random = new BigInteger(temp);
            } while (random < minimum || random > maximum);
            return random;
        }

        /// <summary>
        /// Получает максимально возможное положительное число с заданной длиной бит
        /// </summary>
        /// <param name="length">Длина в битах</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static BigInteger GetMaximumByLength(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException("Длина должна быть положительной");
            }
            BitArray t = new BitArray(length);
            t.SetAll(true);
            byte[] ret = new byte[(t.Length - 1) / 8 + 1];
            t.CopyTo(ret, 0);
            BigInteger Result = new BigInteger(ret.Concat(new byte[] {0}).ToArray());
            return Result;
        }
        /// <summary>
        /// Получает минимально возможное положительное число с заданной длиной бит
        /// </summary>
        /// <param name="length">Длина в битах</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static BigInteger GetMinimumByLength(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException("Длина должна быть положительной");
            }
            BitArray t = new BitArray(length);
            t.SetAll(false);
            t.Set(length - 1, true);
            byte[] ret = new byte[(t.Length - 1) / 8 + 1];
            t.CopyTo(ret, 0);
            BigInteger Result = new BigInteger(ret.Concat(new byte[] { 0 }).ToArray());
            return Result;
        }

        /// <summary>
        /// Получает криптографически стойкое случайное число с заданной длиной в битах
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static BigInteger GetRandomByLength(int length)
        {
            return GetRandomInRange(GetMinimumByLength(length), GetMaximumByLength(length));
        }

        //public static BigInteger GetPrimeByLength(int length)
        //{
        //}

        /// <summary>
        /// Генерирует криптографически стойкое случайное простое число в выбранном диапазоне, с использованием выбранного алгоритма проверки числа на простоту. 
        /// </summary>
        /// <param name="minimum">Нижний предел</param>
        /// <param name="maximum">Верхний предел</param>
        /// <param name="verifier">Реализация PrimeNumberVerifier. Если не указано, будет выбран алгоритм проверки по умолчанию</param>
        /// <returns></returns>
        public static BigInteger GetPrimeInRangeAsync(BigInteger minimum, BigInteger maximum, PrimeNumberVerifier verifier = null)
        {
            if (verifier == null)
            {
                verifier = PrimeNumberVerifier.DefaultPrimeVerifier;
            }
            BigInteger a;
            bool res;
            do
            {
                a = GetRandomInRange(minimum, maximum);
                res = verifier.Test(a);
            } while (!res);
            return a;
        }
        /// <summary>
        /// Генерирует криптографически стойкое случайное простое число в заданном диапазоне с использованием выбранного алгоритма проверки числа на простоту
        /// </summary>
        /// <param name="length">Длина числа в битах</param>
        /// <param name="verifier">Реализация PrimeNumberVerifier. Если не указано, будет выбран алгоритм проверки по умолчанию</param>
        /// <returns></returns>
        public static BigInteger  GetPrimeByLengthAsync(int length, PrimeNumberVerifier verifier = null)
        {
            BigInteger min = GetMinimumByLength(length);
            BigInteger max = GetMaximumByLength(length);
            BigInteger Result = GetPrimeInRangeAsync(min, max, verifier);
            return Result;
        }


    }
}
