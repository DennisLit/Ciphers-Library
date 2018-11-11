using System;
using System.Numerics;

namespace CiphersLibrary.Algorithms 
{ 
    public static class NumericAlgorithms
    {

        public static int GenerateCoprime(int primeInteger)
        { 
            int result = 0;

            for (int i = 2; i < primeInteger; ++i)
            {
                if (Gcd(primeInteger, i) == 1)
                {
                    result = i;
                    break;
                }
            }

            return result;
        }

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

        public static int Gcd(int a, int b)
        {
            int temp = 0;
            if (a < b)
            {
                temp = a;
                a = b;
                b = temp;
            }
            while (b != 0)
            {
                temp = a % b;
                a = b;
                b = temp;
            }
            return a;
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

        public static int ExtendedGCD(int a, int b)
        {
            int d0 = a, d1 = b;
            int x0 = 1, x1 = 0;
            int y0 = 0, y1 = 1;
            while (d1 > 1)
            {
                int q = d0 / d1;
                int d2 = d0 % d1;
                int x2 = x0 - q * x1;
                int y2 = y0 - q * y1;
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

    }
}
