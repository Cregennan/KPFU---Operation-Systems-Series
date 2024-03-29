﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Cregennan.Cryptography.Numerics
{
    public interface IPrimeVerifier
    {
       bool Test(BigInteger n);
       double TestAccuracy(BigInteger n);
    }
}
