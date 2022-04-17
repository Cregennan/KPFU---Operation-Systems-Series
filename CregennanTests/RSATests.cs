using Cregennan.Cryptography.Algorithms.RSA;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cregennan.Cryptography.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CregennanTests
{
    [TestClass]
    public class RSATests
    {

        [TestMethod]
        public void PublicKeyTest()
        {
            var one = BigInteger.Parse("9812798421985211298698362181985412598723798623");
            var two = BigInteger.Parse("91769871958792985798988718287576736178658715");



            RSAPrivateKey key = RSAKey.FromRaw<RSAPrivateKey>(one, two);
            





            var str = key.Encode();
            
            Console.WriteLine(str);

            var key2 = RSAKey.TryFromEncoded<RSAPrivateKey>(str);

            (var e1, var n1) = key.KeyPair;
            (var e2, var n2) = key2.KeyPair;


            Assert.AreEqual(e1, e2);
            Assert.AreEqual(n1, n2);

        }

        [TestMethod]
        public void PrimeGeneratorTest()
        {
            var t = Utils.GetPrimeByLength(10);
            Console.WriteLine(t);

            var x = 10 / 3;
            Console.WriteLine(x);

            var n = Utils.GetPrimeByLength(10/3);
            Console.WriteLine(n);

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ProcessTest()
        {
            byte[] message = { 12, 132, 213, 133, 228, 14, 88 };
            
            (var pub, var pri) = RSACryptoService.GenerateKeyPair(100);


            byte[] crypt = RSACryptoService.Encrypt(message, pub);
            byte[] decrypt = RSACryptoService.Decrypt(crypt, pri);

            CollectionAssert.AreEqual(decrypt, message);
        }


    }
}
