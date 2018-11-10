using CryptoCore.Algorithms;
using CryptoCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCore.Core
{ 
    interface IKeyGeneratorsFactory
    {
        IKeyGenerator NewKeyGenerator(KeyGenerators keyGeneratorType);
    }
}
