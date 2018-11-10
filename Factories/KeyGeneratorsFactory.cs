using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoCore.Algorithms;
using CryptoCore.Data;

namespace CryptoCore.Core
{
    public class KeyGeneratorsFactory : IKeyGeneratorsFactory
    {
        public IKeyGenerator NewKeyGenerator(KeyGenerators keyGeneratorType)
        {
            switch(keyGeneratorType)
            {
                case KeyGenerators.LFSR:
                    return new LFSR();
                case KeyGenerators.Geffe:
                    return new Geffe();
                default:
                    return new LFSR();
            }
        }
    }
}
