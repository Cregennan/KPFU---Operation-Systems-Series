using Cregennan.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Cregennan.Cryptography.Algorithms.RSA
{
    public abstract class RSAKey
    {
        public (BigInteger, BigInteger) KeyPair { get; protected set; }

        public abstract (string, string, string) GetSignature();

        public static T TryFromEncoded<T>(String publickey) where T : RSAKey, new()
        {
            T t = new T();

            (string KEY_BEGIN, string KEY_END, string KEY_DIVIDER) = t.GetSignature();
            publickey = publickey.Trim();
           
            if (!publickey.StartsWith(KEY_BEGIN)) throw new RSAInvalidKeyException();
            publickey = publickey.Replace(KEY_BEGIN, "");

            if (!publickey.EndsWith(KEY_END)) throw new RSAInvalidKeyException();
            publickey = publickey.Replace(KEY_END, "");


            if (!publickey.Contains(KEY_DIVIDER)) throw new RSAInvalidKeyException();

            var en = publickey.Split(new string[] { KEY_DIVIDER }, StringSplitOptions.RemoveEmptyEntries);
            if (en.Length != 2) throw new RSAInvalidKeyException();
            if (en[0].Length == 0 || en[1].Length == 0) throw new RSAInvalidKeyException();


            try
            {
                var e = Convert.FromBase64String(en[0]);
                var n = Convert.FromBase64String(en[1]);
                t.KeyPair = (new BigInteger(e), new BigInteger(n));
                return t;

            } catch (Exception e)
            {
                throw new RSAInvalidKeyException();
            }
        }

        public String Encode()
        {
            (var ee, var nn) = KeyPair;
            var e = Convert.ToBase64String(ee.ToByteArray());
            var n = Convert.ToBase64String(nn.ToByteArray());
            (var kb, var ke, var kd) = GetSignature();
            return kb + e + kd + n + ke;
        }

        protected RSAKey()
        {

        }

        public static T FromRaw<T>(BigInteger first, BigInteger second) where T : RSAKey, new()
        {
            return new T
            {
                KeyPair = (first, second),
            };
        }

    }
    public class RSAPublicKey : RSAKey
    {
        public override (string, string, string) GetSignature() => ("%IMSORRY%", "%SORRYFORWHAT%", "%CENTRE%");
    }
    public class RSAPrivateKey : RSAKey
    {
        public override (string, string, string) GetSignature() => ("%OURDADDY%", "%TOLDUS%", "%CENTRE%");
    }


}
