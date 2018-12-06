using CiphersLibrary.Extension_Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CiphersLibrary.Algorithms
{
    public class Sha1 : IHashFunction
    {
        #region Private props
        private readonly uint[] InitialHashes = new uint[5];

        private long BlocksCount { get; set; }

        private long MessageLength { get; set; }

        private byte[] Message { get; set; }

        #endregion

        #region Public methods

        public BigInteger IntHash(byte[] message)
        {
            var result = GenerateHash(message);
            return new BigInteger(result);
        }

        public string HexHash(byte[] message)
        {
            var result = GenerateHash(message);
            return new BigInteger(result).ToString("X");
        }

        #endregion

        #region Private methods

        private void InitHashValues()
        {
            InitialHashes[0] = 0x67452301;
            InitialHashes[1] = 0xEFCDAB89;
            InitialHashes[2] = 0x98BADCFE;
            InitialHashes[3] = 0x10325476;
            InitialHashes[4] = 0xC3D2E1F0;
        }

        private byte[] GenerateHash(byte[] message)
        {
            //pre-processing procedure
            var resultArray = InitBlocks(message);

            InitHashValues();

            for (int i = 0; i < BlocksCount; i++)
            {
                uint[] w = new uint[80];

                //break chunk into 16 32-bit big-endian words
                for (int j = 0; j < 16; j++)
                {
                    w[j] = ((uint)(resultArray[(i * 512 / 8) + 4 * j] << 24) & 0xFF000000) | ((uint)(resultArray[i * 512 / 8 + 4 * j + 1] << 16) & 0x00FF0000);
                    w[j] |= ((uint)(resultArray[i * 512 / 8 + 4 * j + 2] << 8) & 0xFF00) | (resultArray[i * 512 / 8 + 4 * j + 3] & (uint)0xFF);
                }

                // extend 16 words into 80 words
                for (int j = 16; j < 80; j++)
                {
                    w[j] = (w[j - 3] ^ w[j - 8] ^ w[j - 14] ^ w[j - 16]).RotateLeft(1);
                }

                // initialize initial values
                uint a = InitialHashes[0];
                uint b = InitialHashes[1];
                uint c = InitialHashes[2];
                uint d = InitialHashes[3];
                uint e = InitialHashes[4];
                uint f = 0;
                uint k = 0;

                //main loop
                for (int j = 0; j < 80; j++)
                {
                    if (j <= 19)
                    {
                        f = (b & c) | ((~b) & d);
                        k = 0x5A827999;
                    }
                    else if (j <= 39)
                    {
                        f = b ^ c ^ d;
                        k = 0x6ED9EBA1;
                    }
                    else if (j <= 59)
                    {
                        f = (b & c) | (b & d) | (c & d);
                        k = 0x8F1BBCDC;
                    }
                    else
                    {
                        f = b ^ c ^ d;
                        k = 0xCA62C1D6;
                    }

                    uint temp = a.RotateLeft(5) + f + e + k + w[j];
                    e = d;
                    d = c;
                    c = b.RotateLeft(30);
                    b = a;
                    a = temp;
                }

                InitialHashes[0] += a;
                InitialHashes[1] += b;
                InitialHashes[2] += c;
                InitialHashes[3] += d;
                InitialHashes[4] += e;

            }

            // RESULT
            byte[] hash = new byte[20];
            for (int j = 0; j < 4; j++)
            {
                hash[j] = (byte)((InitialHashes[0] >> 24 - j * 8) & 0xFF);
    
            }
            for (int j = 0; j < 4; j++)
            {
                hash[j + 4] = (byte)((InitialHashes[1] >> 24 - j * 8) & 0xFF);
            }
            for (int j = 0; j < 4; j++)
            {
                hash[j + 8] = (byte)((InitialHashes[2] >> 24 - j * 8) & 0xFF);
            }
            for (int j = 0; j < 4; j++)
            {
                hash[j + 12] = (byte)((InitialHashes[3] >> 24 - j * 8) & 0xFF);

            }
            for (int j = 0; j < 4; j++)
            {
                hash[j + 16] = (byte)((InitialHashes[4] >> 24 - j * 8) & 0xFF);
            }

            return hash;


        }

        private byte[] InitBlocks(byte[] message)
        {
            MessageLength = message.LongLength;
            long bitsLength = MessageLength * 8;

            byte[] Message = new byte[MessageLength + 1];
            Array.Copy(message, 0, Message, 0, MessageLength);
            //Append one
            Message[Message.Length - 1] = 0x80;

            long newBitsLength = Message.Length * 8;
            //append n amount of zero bytes
            while (newBitsLength % 512 != 448)
            {
                newBitsLength += 8;
            }

            byte[] appendedZeroesArray = new byte[newBitsLength / 8];
            Array.Copy(Message, 0, appendedZeroesArray, 0, Message.Length);

            byte[] resultArray = new byte[appendedZeroesArray.Length + 8];
            //append original message length as 64 bit number(big-endian)
            for (int i = 0; i < 8; i++)
            {
                resultArray[resultArray.Length - 1 - i] = (byte)(((uint)bitsLength >> (8 * i)) & 0xFF);
            }

            Array.Copy(appendedZeroesArray, 0, resultArray, 0, appendedZeroesArray.Length);

            MessageLength = resultArray.Length;
            BlocksCount = MessageLength * 8 / 512;

            return resultArray;
        }

        #endregion
    }
}
