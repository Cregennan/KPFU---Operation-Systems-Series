using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cregennan.Core.Exceptions
{
    public class RSAInvalidKeyException : Exception
    {
        public RSAInvalidKeyException() : base("Введен неверный ключ")
        {

        }
    }
}
