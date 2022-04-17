﻿using Cregennan.Core.Exceptions;
using Cregennan.Cryptography.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Cregennan.Cryptography.Algorithms.RSA
{
    public class RSACryptoService
    {
       private RSACryptoService()
        {

        }

        /// <summary>
        /// Генерирует пару ассиметричных ключей заданной длины
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <exception cref="RSAInvalidKeyLengthException"></exception>
        public static (RSAPublicKey, RSAPrivateKey) GenerateKeyPair(int length)
        {
            if (length < 10 || length > 512)
            {
                throw new RSAInvalidKeyLengthException();
            }

            var p = Utils.GetPrimeByLength(length);
            var q = Utils.GetPrimeByLength(length);

            var N = p * q;

            var phi = (p - BigInteger.One) * (q - BigInteger.One);

            var e = BigInteger.One;

            while(phi % e == 0)
            {
                e = Utils.GetPrimeByLength((2 * length) / 3);
            }

            (var x, var y, var gcd) = Utils.ExtendedGCD(phi, e);

            var d = y < 0 ? y + phi : y;

            return (RSAKey.FromRaw<RSAPublicKey>(N, e), RSAKey.FromRaw<RSAPrivateKey>(N, d));
        }


        protected static byte[] Process(byte[] data,  RSAKey key)
        {
            (var a, var b) = key.KeyPair;
            BigInteger t = new BigInteger(data);
            return BigInteger.ModPow(t, b, a).ToByteArray();
        }

        public static byte[] Encrypt(byte[] message, RSAPublicKey key) => Process(message, key);

        public static byte[] Decrypt(byte[] cypher, RSAPrivateKey key) => Process(cypher, key);




    }
}