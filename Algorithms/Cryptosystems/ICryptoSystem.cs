using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CiphersLibrary.Algorithms
{
    interface IRabinCryptoSystem
    {
        bool Initialize(BigInteger firstParam, BigInteger secondParam);

        bool Encrypt(BigInteger p, BigInteger q, string filePath);

        bool Decrypt(BigInteger p, BigInteger q, string filePath);
    }  
}
