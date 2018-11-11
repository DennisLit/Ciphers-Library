using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiphersLibrary.Algorithms
{
    interface IYarmolikRabinCryptoSystem
    {
        void Encrypt(int p, int q, int b, string filePath);

        void Decrypt(int p, int q, int b, string filePath);
    }
}
