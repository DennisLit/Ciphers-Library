using CiphersLibrary.Algorithms;
using CiphersLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiphersLibrary.Core
{ 
    interface IKeyGeneratorsFactory
    {
        IKeyGenerator NewKeyGenerator(KeyGenerators keyGeneratorType);
    }
}
