using CiphersLibrary.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CiphersLibrary.Algorithms
{
    public class YarmolikHash : IHashFunction
    {
        public YarmolikHash(BigInteger modulus)
        {
            this.modulus = modulus;
        }

        private readonly BigInteger startHashValue = 100;

        private readonly BigInteger modulus;

        private BigInteger GenerateHash(byte[] message)
        {
            var returnValue = startHashValue; 

            for (int i = 0; i < message.Length; ++i)
            {
                returnValue = NumericAlgorithms.FastExp(returnValue + (BigInteger)message[i], 2, modulus);
                var x = returnValue.ToString();
            }

            return returnValue;
        }

        public BigInteger IntHash(byte[] message)
        {
            return GenerateHash(message);
        }

        public string HexHash(byte[] message)
        {
            return GenerateHash(message).ToString("X");
        }
    }
}
