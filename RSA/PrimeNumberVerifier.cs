using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RSA
{
    public abstract class PrimeNumberVerifier : IPrimeVerifier
    {
        public static PrimeNumberVerifier DefaultPrimeVerifier()
        {
            return new ParallelMillerRabinVerifier();
        }

        public abstract double TestAccuracy(BigInteger n);

        public readonly bool IsDetermenistic = false;

        public static readonly int[] DefaultWitnesses = { 2, 3, 5 };

        public static bool IsDefaultWitness(BigInteger n)
        {
            foreach(var witness in DefaultWitnesses)
            {
                if (witness == n)
                    return true;
            }
            return false;
        }
        public static bool CheckDefaultWithnesses(BigInteger n)
        {
            foreach(var t in DefaultWitnesses)
            {
                if (n % t == 0 && n != t)
                {
                    return false;
                }
            }
            return true;
        }

        protected int DefaultTestRounds(BigInteger n) => (int)BigInteger.Log(n, 2) + 1;

        protected RandomNumberGenerator generator = RandomNumberGenerator.Create();
        
        /// <summary>
        /// Проверяет, является ли число возможно-простым. Алгоритм проверки зависит от выбранной реализации класса.
        /// </summary>
        /// <param name="n">Число для проверки</param>
        /// <returns>true - число является возможно-простым. false - число составное</returns>
        public abstract bool Test(BigInteger n);


    }
}
