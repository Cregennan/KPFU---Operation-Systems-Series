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

            if (IsDefaultWitness(p))
            {
                return true;
            }
            if (!CheckDefaultWithnesses(p))
            {
                return false;
            }


            for (int i = 0; i < Rounds; i++)
            {
                
                BigInteger a = Utils.GetRandomInRange(2, p);
                
                if (Utils.GCD(a, p) != 1)
                {
                    return false;
                }
                
                BigInteger j =  a.ModPower((p - 1) >> 1, p);
                BigInteger J = (Utils.JacobiSymbol(a, p) + p) % p;


                if (j != J)
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
