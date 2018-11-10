using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCore.Extension_Methods
{
    public static class UIntExtensions
    {
        public static uint RotateLeft(this uint value, int count)
        {
            return (value << count) | (value >> (32 - count));
        }

        public static uint RotateRight(this uint value, int count)
        {
            return (value >> count) | (value << (32 - count));
        }
    }
}
