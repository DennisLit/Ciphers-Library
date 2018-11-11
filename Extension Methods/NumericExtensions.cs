using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CiphersLibrary.Core
{ 
    public static class NumericExtensions
    {
        public static bool CheckIfPrime(this BigInteger value)
        {
            if (value <= 1) return false;
            if (value == 2) return true;
            if (value % 2 == 0) return false;

            var boundary = Sqrt(value);

            for (long i = 3; i <= boundary; i += 2)
            {
                if (value % i == 0) return false;
            }

            return true;
        }

        public static bool CheckIfPrime(this short value)
        {
            if (value <= 1) return false;
            if (value == 2) return true;
            if (value % 2 == 0) return false;

            var boundary = Sqrt(value);

            for (long i = 3; i <= boundary; i += 2)
            {
                if (value % i == 0) return false;
            }

            return true;
        }

        public static bool CheckIfPrime(this int value)
        {
            if (value <= 1) return false;
            if (value == 2) return true;
            if (value % 2 == 0) return false;

            var boundary = Sqrt(value);

            for (long i = 3; i <= boundary; i += 2)
            {
                if (value % i == 0) return false;
            }

            return true;
        }

        public static BigInteger Sqrt(this BigInteger n)
        {
            if (n == 0) return 0;
            if (n > 0)
            {
                int bitLength = Convert.ToInt32(Math.Ceiling(BigInteger.Log(n, 2)));
                var root = BigInteger.One << (bitLength / 2);

                while (!isSqrt(n, root))
                {
                    root += n / root;
                    root /= 2;
                }

                return root;
            }

            throw new ArithmeticException("NaN");
        }

        private static bool isSqrt(BigInteger n, BigInteger root)
        {
            var lowerBound = root * root;
            var upperBound = (root + 1) * (root + 1);

            return (n >= lowerBound && n < upperBound);
        }
    }
}
