using CiphersLibrary.Algorithms;
using CiphersLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CiphersLibrary.Factories
{
    interface IHashFunctionsFactory
    {
        IHashFunction NewHashFunction(BigInteger modulus, HashFunction hashFuncType);
    }
}
