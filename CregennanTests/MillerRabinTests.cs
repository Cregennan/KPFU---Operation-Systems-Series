using Cregennan.Cryptography.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CregennanTests
{
    [TestClass]
    public class MillerRabinTests
    {
        [TestMethod]
        public void MillerRabinPerformance()
        {
            BigInteger n = BigInteger.Parse("11667094374261657050180388187389677236635408300966846871829843559810295242204638551325955550870342730439727324703470045456195730361411376028573289314906413");
            PrimeNumberVerifier synct = new MillerRabinVerifier();
            PrimeNumberVerifier asynct = new ParallelMillerRabinVerifier();
            int k = 50;


            double syncms = 0;
            for (int i = 0; i < k; i++)
            {
                Stopwatch one = Stopwatch.StartNew();
                synct.Test(n);
                one.Stop();
                syncms += one.Elapsed.TotalMilliseconds;
            }

            double asyncms = 0;
            for (int i = 0; i < k; i++)
            {
                Stopwatch two = Stopwatch.StartNew();
                asynct.Test(n);
                two.Stop();
                asyncms += two.Elapsed.TotalMilliseconds;
            }
            Debug.WriteLine("Обычный Миллер Рабин: " + syncms / k);
            Debug.WriteLine("Параллельный Миллер Рабин: " + asyncms / k);
            Assert.IsTrue(true);
        }

    }
}
