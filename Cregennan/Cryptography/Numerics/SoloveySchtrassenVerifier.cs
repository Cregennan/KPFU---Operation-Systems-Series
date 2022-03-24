using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Cregennan.Cryptography.Numerics
{
    public class SoloveySchtrassenVerifier : PrimeNumberVerifier
    {
        public override bool Test( BigInteger p)
        {
            int Rounds = this.DefaultTestRounds(p);

            for (int i = 0; i < Rounds; i++)
            {
                
                BigInteger a = Utils.GetRandomInRange(2, p);
                Debug.WriteLine(a);
                Debug.WriteLine(p);
                if (Utils.GCD(a, p) != 1)
                {
                    return false;
                }
                BigInteger j = BigInteger.ModPow(a, (p - 1) >> 2, p);
                BigInteger J = Utils.JacobiSymbol(a, p);
                if (j != J)
                {
                    return false;
                }
                else
                {
                    continue;
                }
            }
            return true;
        }
    }
}
