using System;
using System.Numerics;

namespace CiphersLibrary.Algorithms 
{
    public static class NumericAlgorithms
    {

        public static int FastExp(int number, int power, int modulus)
        {
            int a = number;
            int z = power;
            int x = 1;
            while (z != 0)
            {
                while (z % 2 == 0)
                {
                    z /= 2;
                    a = (a * a) % modulus;
                }
                --z;
                x = (x * a) % modulus;
            }
            return x;
        }

        /// <summary>
        /// Extended Euclidean algorithm.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="x">First coefficent</param>
        /// <param name="y">Second coefficent</param>
        /// <returns>Gcd</returns>
        public static int ExtendedGCD(int a, int b, out int x, out int y)
        {
            if (a == 0)
            {
                x = 0; y = 1;
                return b;
            }
            int x1, y1;
            int d = ExtendedGCD(b % a, a, out x1, out y1);
            x = y1 - (b / a) * x1;
            y = x1;
            return d;
        }

        public static BigInteger GCD(BigInteger a, BigInteger b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a == 0 ? b : a;
        }

        /// <summary>
        /// Overload to find multiplicative inversion of b mod a
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigInteger ExtendedGCD(BigInteger a, BigInteger b)
        {
            BigInteger d0 = a, d1 = b;
            BigInteger x0 = 1, x1 = 0;
            BigInteger y0 = 0, y1 = 1;
            while (d1 > 1)
            {
                BigInteger q = d0 / d1;
                BigInteger d2 = d0 % d1;
                BigInteger x2 = x0 - q * x1;
                BigInteger y2 = y0 - q * y1;
                d0 = d1;
                d1 = d2;
                x0 = x1;
                x1 = x2;
                y0 = y1;
                y1 = y2;
            }
            if (y1 < 0)
            {
                y1 += a;
            }
            return y1;
        }

        public static BigInteger FastExp(BigInteger number, BigInteger power, BigInteger modulus)
        {
            BigInteger a = number;
            BigInteger z = power;
            BigInteger x = 1;
            while (z != 0)
            {
                while (z % 2 == 0)
                {
                    z /= 2;
                    a = (a * a) % modulus;
                }
                --z;
                x = (x * a) % modulus;
            }
            return x;
        }

        /// <summary>
        /// Extended Euclidean algorithm.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="x">First coefficent</param>
        /// <param name="y">Second coefficent</param>
        /// <returns>Gcd</returns>
        public static BigInteger ExtendedGCD(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
        {
            if (a == 0)
            {
                x = 0; y = 1;
                return b;
            }
            BigInteger x1, y1;
            BigInteger d = ExtendedGCD(b % a, a, out x1, out y1);
            x = y1 - (b / a) * x1;
            y = x1;
            return d;
        }

        public static int NegativeMod(int num1, int num2)
        {
            num1 = Math.Abs(num1);
            return (num2 * ((int)Math.Truncate((double)num1 / num2) + 1)) - num1;
        }

    }
}
