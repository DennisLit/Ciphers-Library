using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCore.Core
{
    public static class PrimeNumsGenerator
    {
        /// <summary>
        /// Generates Blum prime number of bitsLength length.
        /// </summary>
        /// <param name="bitsLength"></param>
        /// <returns>Generated number</returns>
        public static BigInteger GeneratePrime(int bitsLength)
        {
            Mono.Math.BigInteger p;

            do
            {
                p = Mono.Math.BigInteger.GeneratePseudoPrime(bitsLength);
            } while (!Mono.Math.BigInteger.Equals(p % 4, 3));

            byte[] p_array = p.GetBytes().Reverse().ToArray();

            if (p > 0 && (p_array[p_array.Length - 1] & 0x80) > 0)
            {
                byte[] temp = new byte[p_array.Length];
                Array.Copy(p_array, temp, p_array.Length);
                p_array = new byte[temp.Length + 1];
                Array.Copy(temp, p_array, temp.Length);
            }

            BigInteger q = new BigInteger(p_array);
            return q;
        }
    }
}
