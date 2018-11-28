using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CiphersLibrary.Algorithms;
using CiphersLibrary.Data;

namespace CiphersLibrary.Factories
{
    public class HashFunctionFactory : IHashFunctionsFactory
    {
        public IHashFunction NewHashFunction(BigInteger modulus, HashFunction hashFuncType)
        {
            switch(hashFuncType)
            {
                case HashFunction.YarmolikHash:
                    return new YarmolikHash(modulus);
                case HashFunction.Sha1:
                    return new Sha1();
                default:
                    return new Sha1();
            }
        }

    }
}
