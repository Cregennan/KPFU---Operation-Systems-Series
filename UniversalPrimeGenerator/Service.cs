using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalPrimeGenerator
{
    public static class Service
    {
        public static string[] Algorythms =
        {
            "Тест Ферма",
            "Тест Миллера-Рабина",
            "Тест Соловея-Штрассена",

        };
        public static Cregennan.Cryptography.Numerics.PrimeNumberVerifier VerifierFactory(int n)
        {
            switch (n)
            {
                case 0:
                    return new Cregennan.Cryptography.Numerics.FermatVerifier();
                case 1:
                    return new Cregennan.Cryptography.Numerics.MillerRabinVerifier();
                case 2:
                    return new Cregennan.Cryptography.Numerics.SoloveySchtrassenVerifier();
                default:
                    return new Cregennan.Cryptography.Numerics.MillerRabinVerifier();
            }
        }
    }
}
