using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CiphersLibrary.Extension_Methods;

namespace CiphersLibrary.Algorithms
{
    public class Sha256 : IHashFunction<uint[]>
    {
        #region Ctor

        public Sha256()
        {
        }

        #endregion

        #region Private fields and props
        /// <summary>
        /// First 32 bits fractional parts of cubic roots of first 64 prime numbers  
        /// </summary>
        private static readonly uint[] K =
        {
                0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5,
                0x3956c25b, 0x59f111f1, 0x923f82a4, 0xab1c5ed5,
                0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3,
                0x72be5d74, 0x80deb1fe, 0x9bdc06a7, 0xc19bf174,
                0xe49b69c1, 0xefbe4786, 0x0fc19dc6, 0x240ca1cc,
                0x2de92c6f, 0x4a7484aa, 0x5cb0a9dc, 0x76f988da,
                0x983e5152, 0xa831c66d, 0xb00327c8, 0xbf597fc7,
                0xc6e00bf3, 0xd5a79147, 0x06ca6351, 0x14292967,
                0x27b70a85, 0x2e1b2138, 0x4d2c6dfc, 0x53380d13,
                0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85,
                0xa2bfe8a1, 0xa81a664b, 0xc24b8b70, 0xc76c51a3,
                0xd192e819, 0xd6990624, 0xf40e3585, 0x106aa070,
                0x19a4c116, 0x1e376c08, 0x2748774c, 0x34b0bcb5,
                0x391c0cb3, 0x4ed8aa4a, 0x5b9cca4f, 0x682e6ff3,
                0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208,
                0x90befffa, 0xa4506ceb, 0xbef9a3f7, 0xc67178f2
        };

        private readonly uint[] InitialHashes = new uint[8];
        private readonly uint[] Words = new uint[64];
        private ulong BlocksCount { get; set; }
        private ulong MessageLength { get; set; }
        private byte[] Message { get; set; }

        #endregion


        public uint[] GenerateHash(byte[] message)
        {
            MessageLength = (ulong)message.LongLength;
            var zeroBitsToAddQty = 512 - (int)((MessageLength * 8 + 1 + 64) % 512);
            Message = new byte[(MessageLength * 8 + 1 + 64 + (ulong)zeroBitsToAddQty) / 8];

            Array.Copy(message, Message, message.LongLength);
            Message[MessageLength] = 128; 
            //set 1-st bit to "1", 7 remaining to "0"

            for (var i = MessageLength + 1; i < (ulong)Message.LongLength - 8; i++)
                Message[i] = 0;

            var messageBitLengthLittleEndian = BitConverter.GetBytes(MessageLength * 8);
            var messageBitLengthBigEndian = new byte[messageBitLengthLittleEndian.Length];

            for (int i = 0, j = messageBitLengthLittleEndian.Length - 1;
                i < messageBitLengthLittleEndian.Length;
                i++, j--)
                messageBitLengthBigEndian[i] = messageBitLengthLittleEndian[j];

            Array.Copy(messageBitLengthBigEndian, 0, Message, Message.LongLength - 8, 8);

            BlocksCount = (ulong)Message.LongLength / 64;
            InitHashValues();

            for (ulong i = 0; i < BlocksCount; i++)
            {
                GenerateWords(i);

                ProcessBlock();
            }

            return InitialHashes;

        }


        #region Private methods

        private void InitHashValues()
        {
            InitialHashes[0] = 0x6a09e667;
            InitialHashes[1] = 0xbb67ae85;
            InitialHashes[2] = 0x3c6ef372;
            InitialHashes[3] = 0xa54ff53a;
            InitialHashes[4] = 0x510e527f;
            InitialHashes[5] = 0x9b05688c;
            InitialHashes[6] = 0x1f83d9ab;
            InitialHashes[7] = 0x5be0cd19;
        }

        private void ProcessBlock()
        {
            var a = InitialHashes[0];
            var b = InitialHashes[1];
            var c = InitialHashes[2];
            var d = InitialHashes[3];
            var e = InitialHashes[4];
            var f = InitialHashes[5];
            var g = InitialHashes[6];
            var h = InitialHashes[7];

            for (int i = 0; i < 64; ++i)
            {
                var t1 = h 
                + ((e.RotateRight(6)) ^ (e.RotateRight(11)) ^ (e.RotateRight(25)))
                + ( e & f) ^ ((~e) & g) + K[i] + Words[i];

                var t2 = a.RotateRight(2) ^ a.RotateRight(13) ^ a.RotateRight(22) + ((a & b) ^ (a & c) ^ (b & c));
                    
                h = g;
                g = f;
                f = e;
                e = d + t1;
                d = c;
                c = b;
                b = a;
                a = t1 + t2;
            }

            InitialHashes[0] += a;
            InitialHashes[1] += b;
            InitialHashes[2] += c;
            InitialHashes[3] += d;
            InitialHashes[4] += e;
            InitialHashes[5] += f;
            InitialHashes[6] += g;
            InitialHashes[7] += h;
        }

        private void GenerateWords(ulong blockNumber)
        {
            for (var i = 0; i < 16; i++)
                Words[i] = Bytes_To_UInt32(Message, blockNumber * 64 + (ulong)i * 4);

            for (var i = 16; i <= 63; i++)
                Words[i] = Words[i - 16]
                + (Words[i - 15].RotateRight(7) ^ Words[i - 15].RotateRight(18) ^ (Words[i - 15] >> 3)
                + Words[i - 7] + Words[i - 2].RotateRight(17) ^ Words[i - 2].RotateRight(19) ^ (Words[i - 2] >> 10));
        }

        #endregion

        internal static uint Bytes_To_UInt32(byte[] bs, ulong off)
        {
            var n = (uint)bs[off] << 24;
            n |= (uint)bs[++off] << 16;
            n |= (uint)bs[++off] << 8;
            n |= bs[++off];
            return n;
        }
    }
}
