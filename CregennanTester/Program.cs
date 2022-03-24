using Cregennan.Cryptography.Numerics;
using System.Diagnostics;
using System.Numerics;

namespace CregennanTester;

public static class Program
{
    public static void Main()
    {
        Stopwatch sw = Stopwatch.StartNew();
        BigInteger rand = Utils.GetPrimeByLengthAsync(2048, new FermatVerifier());
        sw.Stop();
        Console.WriteLine(rand.ToString());
        Console.WriteLine(sw.Elapsed.TotalMilliseconds);
    }
}
