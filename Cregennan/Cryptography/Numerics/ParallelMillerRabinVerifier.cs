using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cregennan.Cryptography.Numerics
{

    public class ParallelMillerRabinVerifier : PrimeNumberVerifier
    {

        private IEnumerable<int> Rounds(int k)
        {
            int diff = Environment.ProcessorCount;
            while(k > 0)
            {
                var ko = k;
                k -= diff;
                yield return ko > diff ? diff : ko;
            }

        }
        private bool MillerRabinSubRoutine(BigInteger n, int k, int s, BigInteger t)
        { 
            for (int i = 0; i < k; i++)
            { 
                BigInteger a = Utils.GetRandomInRange(2, n - 2);
                BigInteger x = BigInteger.ModPow(a, t, n);

                if (x == 1 || x == n - 1)
                    continue;

                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, n);

                    if (x == 1)
                    {
                        return false;
                    }

                    if (x == n - 1)
                        break;
                }

                if (x != n - 1)
                {
                    return false;
                }
            }
            return true;
        }


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

            BigInteger t = n - 1;
            int s = 0;
            while (t % 2 == 0)
            {
                t /= 2;
                s += 1;
            }
            try
            {
                var tt = Rounds(k);
                return tt.AsParallel().Select(x => MillerRabinSubRoutine(n, x, s, t)).All(y => y);
            }
            catch (Exception notprime)
            {
                return false;
            }
        }

        public override double TestAccuracy(BigInteger n)
        {
            var t = Math.Pow(4, -this.DefaultTestRounds(n));
            if (t < 0.00001)
            {
                return 0.999999;
            }

            return 1 - t;
        }
    }
}
