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

    public class MillerRabinVerifier : PrimeNumberVerifier
    {

        private const int ComputationsPerTask = 50;

        CancellationTokenSource cancelSignal = new CancellationTokenSource();
        CancellationToken ct;
        private bool MillerRabinSubRoutine(BigInteger n, int k, int s, BigInteger t)
        {
           
            for (int i = 0; i < k; i++)
            {
                if (ct.IsCancellationRequested)
                {
                    return false;
                }
                byte[] _a = new byte[n.ToByteArray().LongLength];

                BigInteger a = Utils.GetRandomInRange(2, n - 2);
                Stopwatch sw = Stopwatch.StartNew();
                BigInteger x = BigInteger.ModPow(a, t, n);
                sw.Stop();

                if (x == 1 || x == n - 1)
                    continue;

                for (int r = 1; r < s; r++)
                {
                    if (ct.IsCancellationRequested)
                    {
                        return false;
                    }
                    x = BigInteger.ModPow(x, 2, n);

                    if (x == 1)
                        throw new Cregennan.Core.Exceptions.PrimalityTestException("Число составное");

                    if (x == n - 1)
                        break;
                }

                if (x != n - 1)
                    throw new Cregennan.Core.Exceptions.PrimalityTestException("Число составное");
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
            int kk = k;
            //ct = cancelSignal.Token;
            //List<Task<bool>> taskpool = new List<Task<bool>>();
            try
            {
                return MillerRabinSubRoutine(n, k, s, t);
                //while (kk > 0)
                //{
                //    int currentThreadRounds = ComputationsPerTask;
                //    if (currentThreadRounds < kk)
                //    {
                //        kk -= currentThreadRounds;
                //    }
                //    else
                //    {
                //        currentThreadRounds = kk;
                //        kk = 0;
                //    }
                //    Task<bool> task = new Task<bool>(() => MillerRabinSubRoutine(n, currentThreadRounds, s, t), ct);
                //    taskpool.Add(task);
                //}
            
                //foreach(Task<bool> task in taskpool)
                //{
                //    task.Start();
                //}

            }
            catch (Cregennan.Core.Exceptions.PrimalityTestException notprime)
            {
                return false;
            }
        }
    }
}
