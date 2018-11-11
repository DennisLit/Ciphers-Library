using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiphersLibrary.Algorithms
{
    interface IHashFunction<T>
    {
        T GenerateHash(byte[] message);
    }
}
