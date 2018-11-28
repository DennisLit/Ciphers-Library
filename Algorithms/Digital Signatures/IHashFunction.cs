using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CiphersLibrary.Algorithms
{
    public interface IHashFunction
    {
        BigInteger IntHash(byte[] message);
        string HexHash(byte[] message);
    }
}
