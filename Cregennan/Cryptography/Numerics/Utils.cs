using Cregennan.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;

namespace Cregennan.Cryptography.Numerics
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
        /// Генерирует криптографически стойкое случайное простое число в заданном диапазоне с использованием выбранного алгоритма проверки числа на простоту
        /// </summary>
        /// <param name="length">Длина числа в битах</param>
        /// <param name="verifier">Реализация PrimeNumberVerifier. Если не указано, будет выбран алгоритм проверки по умолчанию</param>
        /// <returns></returns>
        public static BigInteger  GetPrimeByLength(int length, PrimeNumberVerifier verifier = null)
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
            while(a > 0 && b > 0)
            {
                if (a > b)
                {
                    a %= b;
                }else
                if (b > a) b %= a;
            }
            return a + b; 
        }


        /// <summary>
        /// Вычисляет символ Лежандра для двух чисел
        /// </summary>
        /// <param name="a"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static int LegendreSymbol(BigInteger a, BigInteger p)
        {
            if (a == BigInteger.One) return 1;
            if (a % 2 == 0)
            {
                return LegendreSymbol(a >> 1, p) * MinusOnePow((p * p - 1) >> 3);
            }
            else
            {
                return LegendreSymbol(p % a, a) * MinusOnePow(((a - 1) * (p - 1)) >> 2);
            }

        }

        /// <summary>
        /// Быстрое возведение -1 в выбранную степень
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private static int MinusOnePow(BigInteger a) => a.IsEven ? 1 : -1;

        /// <summary>
        /// Вычисляет символ Якоби для двух чисел
        /// </summary>
        /// <param name="a"></param>
        /// <param name="n">Нечётное число больше 0</param>
        /// <returns></returns>
        public static int JacobiSymbol(BigInteger a, BigInteger n)
        {

            if (n <= 0) throw new Exception("n должно быть больше 0");
            if (n % 2 == 0) throw new Exception("n должно быть нечетным");
            a %= n;

            int result = 1;
            while (a != 0)
            {
                while (a % 2 == 0)
                {
                    a /= 2;
                    BigInteger n_mod_8 = n % 8;
                    if (n_mod_8 == 3 || n_mod_8 == 5)
                    {   
                        result = -result;
                    }
                }
                (a, n) = (n, a);
                if (a % 4 == 3 && n % 4 == 3)
                {
                    result = -result;
                }
                a %= n;
            }
            if (n == 1)
            {
                return result;
            }
            return 0;

        }

        /// <summary>
        /// 16битный Примитивный корень числа
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static BigInteger PrimitiveRoot16(this BigInteger n)
        {
            List<int> a = new List<int>();
            BigInteger phi = n - 1;

            List<BigInteger> factors = GetFactors(phi);

            for (BigInteger r = Minimum16; r <= Maximum16; r++)
            {
                bool flag = false;
                foreach (BigInteger aa in factors)
                {
                    if (BigInteger.ModPow(r, phi / aa, n) == 1)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag) return r;
            }
            return -1;
        }

        /// <summary>
        /// Возвращает список делителей числа 
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static List<BigInteger> GetFactors(this BigInteger n)
        {
            List<BigInteger> factors = new List<BigInteger>();
            BigInteger x = 2;
            while (x <= n)
            {
                if (n % x == 0)
                {
                    factors.Add(x);
                    n = n / x;
                }
                else
                {
                    x++;
                    if (x * x >= n)
                    {
                        factors.Add(n);
                        break;
                    }
                }
            }
            return factors;
        }

        /// <summary>
        /// Выполняет быстрое возведение положительного числа в положительную степень по модулю
        /// </summary>
        /// <param name="a">Число для возведения в степени</param>
        /// <param name="exponent">Степень</param>
        /// <param name="modulus">Модуль числа</param>
        /// <returns></returns>
        public static BigInteger ModPower(this BigInteger a, BigInteger exponent, BigInteger modulus)
        {
            BigInteger Result = 1;
            BigInteger Bit = a % modulus;

            while (exponent > 0)
            {
                if ((exponent & 1) == 1)
                {
                    Result *= Bit;
                    Result %= modulus;
                }
                Bit *= Bit;
                Bit %= modulus;
                exponent >>= 1;
            }
            return Result;       
        }
    }
}
