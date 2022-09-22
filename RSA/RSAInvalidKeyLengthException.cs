using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA
{
    public class RSAInvalidKeyLengthException : Exception
    {
        public RSAInvalidKeyLengthException() : base("Неверная длина ключа")
        {

        }
    }
}

