using System;
using System.Numerics;

namespace Cregennan.Cryptography.Numerics
{
    public class FermatVerifier : PrimeNumberVerifier
    {
        public override bool Test(BigInteger n)
        {
            if (n < 2)
            {
                throw new ArgumentException("Аргумент должен быть не меньше 2");
            }
            if (!PrimeNumberVerifier.CheckDefaultWithnesses(n))
                return false;
            if (PrimeNumberVerifier.IsDefaultWitness(n))
            {
                return true;
            }

            int k = this.DefaultTestRounds(n);
            
            for(int i = 0; i < k; i++)
            {

                BigInteger a = n.GetCoprimeInRange(2, n - 2);

                BigInteger t = BigInteger.ModPow(a, n - 1, n);
                if (t != 1)
                {
                    return false;
                }
            }
            return true;
        }

        public override double TestAccuracy(BigInteger n)
        {
            return 1 - Math.Pow(2, -this.DefaultTestRounds(n));
        }
    }
}
