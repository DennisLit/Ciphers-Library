using CiphersLibrary.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiphersLibrary.Algorithms
{
    public class YarmolikHash : IHashFunction<int>
    {
        public YarmolikHash(int modulus)
        {
            this.modulus = modulus;
        }

        private readonly int startHashValue = 100;

        private readonly int modulus;

        public int GenerateHash(byte[] message)
        {
            var returnValue = 0; 

            for (int i = 0; i < message.Length; ++i)
            {
                returnValue = NumericAlgorithms.FastExp(startHashValue + message[i], 2, modulus);
            }

            return returnValue;
        }
    }
}
