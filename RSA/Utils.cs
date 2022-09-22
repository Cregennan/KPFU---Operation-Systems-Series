using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RSA
{
    public static class Utils
    {
        /// <summary>
        /// Криптографически стойкий генератор случайных чисел
        /// </summary>
        public static RandomNumberGenerator generator = RandomNumberGenerator.Create();

        public static readonly UInt64 Minimum64 = 0x8000000000000000;
        public static readonly UInt64 Maximum64 = 0xffffffffffffffff;
        public static readonly UInt32 Minimum32 = 0x80000000;
        public static readonly UInt32 Maximum32 = 0xffffffff;
        public static readonly int Minimum16 = 0x8000;
        public static readonly int Maximum16 = 0xffff;





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
            byte[] ret = t.ToBytes().ToArray();

            BigInteger Result = new BigInteger(ret.Concat(new byte[] { 0 }).ToArray());
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
            byte[] ret = t.ToBytes().ToArray();

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


        /// <summary>
        /// Генерирует криптографически стойкое случайное простое число в выбранном диапазоне, с использованием выбранного алгоритма проверки числа на простоту. 
        /// </summary>
        /// <param name="minimum">Нижний предел</param>
        /// <param name="maximum">Верхний предел</param>
        /// <param name="verifier">Реализация PrimeNumberVerifier. Если не указано, будет выбран алгоритм проверки по умолчанию</param>
        /// <returns></returns>
        public static BigInteger GetPrimeInRange(BigInteger minimum, BigInteger maximum, PrimeNumberVerifier verifier = null)
        {
            if (verifier == null)
            {
                verifier = PrimeNumberVerifier.DefaultPrimeVerifier();
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
        /// Возвращает случайное взаимно простое  число для данного числа в пределах от min до max
        /// <param name="number"></param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns></returns>
        public static BigInteger GetCoprimeInRange(this BigInteger number, BigInteger minimum, BigInteger maximum)
        {
            BigInteger a;
            do
            {
                a = GetRandomInRange(minimum, maximum);
            } while (BigInteger.GreatestCommonDivisor(a, number) != BigInteger.One);
            return a;
        }



        /// <summary>
        /// Расширенный алгоритм Евклида
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static (BigInteger, BigInteger, BigInteger) ExtendedGCD(BigInteger a, BigInteger b)
        {
            (var old_r, var r) = (a, b);
            (var old_s, var s) = (BigInteger.One, BigInteger.Zero);
            (var old_t, var t) = (BigInteger.Zero, BigInteger.One);
            while (r != 0)
            {
                var quotient = old_r / r;
                (old_r, r) = (r, old_r - quotient * r);
                (old_s, s) = (s, old_s - quotient * s);
                (old_t, t) = (t, old_t - quotient * t);
            }
            return (old_s, old_t, old_r);
        }

        /// <summary>
        /// Генерирует криптографически стойкое случайное простое число в заданном диапазоне с использованием выбранного алгоритма проверки числа на простоту
        /// </summary>
        /// <param name="length">Длина числа в битах</param>
        /// <param name="verifier">Реализация PrimeNumberVerifier. Если не указано, будет выбран алгоритм проверки по умолчанию</param>
        /// <returns></returns>
        public static BigInteger GetPrimeByLength(int length, PrimeNumberVerifier verifier = null)
        {
            BigInteger min = GetMinimumByLength(length);
            BigInteger max = GetMaximumByLength(length);
            BigInteger Result = GetPrimeInRange(min, max, verifier);
            return Result;
        }

        /// <summary>
        /// Вычисляет наибольший общий делитель двух чисел
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigInteger BinaryGCD(BigInteger a, BigInteger b)
        {
            if (a == b) return a;
            if (a == 0) return b;
            if (b == 0) return a;
            if (a == 1) return 1;
            if (b == 1) return 1;

            if (a % 2 == 0 && b % 2 == 0) return 2 * BinaryGCD(a >> 1, b >> 1);

            if (a % 2 == 0 && b % 2 != 0) return BinaryGCD(a >> 1, b);

            if (a % 2 != 0 && b % 2 == 0) return BinaryGCD(a, b >> 1);
            if (a % 2 != 0 && b % 2 != 0)
            {
                if (a > b)
                {
                    return BinaryGCD((a - b) >> 1, a);
                }
                else
                {
                    return BinaryGCD((b - a) >> 1, b);
                }
            }
            return 1;
        }

        public static BigInteger GCD(BigInteger a, BigInteger b)
        {
            while (a > 0 && b > 0)
            {
                if (a > b)
                {
                    a %= b;
                }
                else
                if (b > a) b %= a;
            }
            return a + b;
        }


        
        

    }
}
