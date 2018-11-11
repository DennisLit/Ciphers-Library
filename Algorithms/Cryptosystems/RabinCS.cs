using CiphersLibrary.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CiphersLibrary.Algorithms
{
    /// <summary>
    /// Rabin Cryptosystem. 
    /// Works slow on keys below ~ 100 bits
    /// </summary>
    public class RabinCS : IRabinCryptoSystem
    {
        #region Private fields

        /// <summary>
        /// Bytes that we'll set to zero to recognize our message from 4 different roots
        /// So, our key length must be higher than this number
        /// </summary>
        private const int bytesHeaderAmount = 8;

        /// <summary>
        /// Public key 
        /// </summary>
        private static BigInteger PublicKey { get; set; }

        /// <summary>
        /// Length of public key in bytes . Must be more than <see cref="bytesHeaderAmount"/>
        /// </summary>
        private static int KeyLength { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Initializes the instance of <see cref="RabinCS"/> class.
        /// </summary>
        /// <param name="firstParam">secret key p</param>
        /// <param name="secondParam">secret key q</param>
        public bool Initialize(BigInteger firstParam, BigInteger secondParam)
        {
            var tmpPublicKey = firstParam * secondParam;

            var tmpLength = GetKeyByteLength(tmpPublicKey);

            if (tmpLength < bytesHeaderAmount + 1)
                return false;

            PublicKey = tmpPublicKey;

            KeyLength = tmpLength;

            return true;
        }

        /// <summary>
        /// Encrypts a file according to Rabin's definition 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public bool Encrypt(BigInteger p, BigInteger q, string filePath)
        {
            byte[] MessageBuf;

            try
            {
                using (var FsStream = new FileStream(filePath, FileMode.Open))
                using (var EnStream = new FileStream(GetOutputEncryptedPath(filePath), FileMode.Create))
                using (var sWriter = new StreamWriter(EnStream))
                using (var binStream = new BinaryReader(FsStream))
                {
                    MessageBuf = EncryptFileChunk(binStream, KeyLength);

                    while (MessageBuf.Length > 0)
                    {
                        sWriter.Write(new BigInteger(Encrypt(p, q, MessageBuf)) + ":");

                        MessageBuf = EncryptFileChunk(binStream, KeyLength);
                    }


                }

                return true;
            }

            catch(ArgumentException)
            {
                return false;
            }
        }

        /// <summary>
        /// Decrypts a file encrypted with Encrypt method from this class
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        public bool Decrypt(BigInteger p, BigInteger q, string filePath)
        {
            try
            {
                using (var FsStream = new FileStream(filePath, FileMode.Open))
                using (var DecStream = new FileStream(GetOutputDecryptedPath(filePath), FileMode.Create, FileAccess.Write))
                using (var bWriter = new BinaryWriter(DecStream))
                using (var sr = new StreamReader(FsStream))
                {
                    var encryptedChunk = string.Empty;

                    while (!string.IsNullOrEmpty(encryptedChunk = GetEncryptedChunk(sr)))
                    {

                        var results = Decrypt(p, q, BigInteger.Parse(encryptedChunk));

                        bWriter.Write(GetDecryptedChunk(results));

                    }
                }

                return true;
            }

            catch
            {
                return false;
            }
            

        }


        #endregion

        #region Core methods

        /// <summary>
        /// Encrypts a message
        /// </summary>
        /// <param name="FirstPrime"> First prime (part of secret key)</param>
        /// <param name="SecondPrime">Second prime (part of secret key)</param>
        /// <param name="Message">Message to encrypt</param>
        /// <returns></returns>
        private byte[] Encrypt(BigInteger FirstPrime, BigInteger SecondPrime, byte[] plainText)
        {
            return NumericAlgorithms.FastExp(new BigInteger(plainText), 2, CalculatePublicKey(FirstPrime, SecondPrime)).ToByteArray();
        }

        private BigInteger[] Decrypt(BigInteger FirstPrime, BigInteger SecondPrime, BigInteger cipherText)
        {
            var publicKey = CalculatePublicKey(FirstPrime, SecondPrime);

            if (cipherText > publicKey)
                throw new ArgumentException("c", "Ciphertext is larger than public key");

            BigInteger r = NumericAlgorithms.FastExp(cipherText, (FirstPrime + 1) / 4, FirstPrime);
            BigInteger s = NumericAlgorithms.FastExp(cipherText, (SecondPrime + 1) / 4, SecondPrime);

            BigInteger a, b;
            NumericAlgorithms.ExtendedGCD(FirstPrime, SecondPrime, out a, out b);

            //(a*p*r + b*q*s) % n

            BigInteger m1 = (a * FirstPrime * s + b * SecondPrime * r) % publicKey;
            if (m1 < 0)
                m1 += publicKey;
            BigInteger m2 = publicKey - m1;
            BigInteger m3 = (a * FirstPrime * s - b * SecondPrime * r) % publicKey;
            if (m3 < 0)
                m3 += publicKey;
            BigInteger m4 = publicKey - m3;
            return new BigInteger[] { m1, m2, m3, m4 };

        }

        #endregion

        #region Misc
        /// <summary>
        /// Sets the length of a key in bytes
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        public static int GetKeyByteLength(BigInteger PublicKey)
        {
            int bitLength = (int)(BigInteger.Log(PublicKey) / BigInteger.Log(2));

            return bitLength % 8 == 0 ? bitLength / 8 - 1 : bitLength / 8;
        }

        private BigInteger CalculatePublicKey(BigInteger firstInt, BigInteger secondInt)
        {
            return firstInt * secondInt;
        }


        #endregion

        #region Helper path methods

        private static string GetOutputEncryptedPath(string InputPath)
        {
            return InputPath.Insert(InputPath.LastIndexOf('.'), "Encrypted");
        }

        private static string GetOutputDecryptedPath(string InputPath)
        {
            return InputPath.Insert(InputPath.LastIndexOf('.'), "Decrypted");
        }

        #endregion

        #region Encryption/Decryption helper methods

        /// <summary>
        /// Reads all symbols in stream until the delimiter is found. 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>Symbols that was read</returns>
        public static string GetEncryptedChunk(StreamReader reader)
        {
            string chunk = string.Empty;
            int currSymbol = 0;

            while ((currSymbol = reader.Read()) != ':')
            {
                //if reader have reached the end of stream it returns -1.

                if (currSymbol == -1)
                    return null;

                chunk += (char)currSymbol;
            }

            return chunk;
        }

        /// <summary>
        /// Reads a file chunk , 
        /// sets bytesHeaderAmount amount of bytes to zero
        /// and sets last byte to one
        /// </summary>
        /// <param name="br">Stream that sequentially reads the file</param>
        /// <param name="byteLength">Length of Public key in bytes because this is the only thing we know.</param>
        /// <returns></returns>
        private static byte[] EncryptFileChunk(BinaryReader br, int byteLength)
        {
            if (byteLength - bytesHeaderAmount < 1)
                throw new ArgumentException("byteLength", "Wrong length of key in bytes. Try using larger values");

            byte[] temp = br.ReadBytes(byteLength - bytesHeaderAmount);

            if (temp.Length == 0)
                return new byte[0];

            byte[] chunk = new byte[temp.Length + bytesHeaderAmount];

            Array.Copy(temp, 0, chunk, bytesHeaderAmount - 1, temp.Length);

            //last byte == 1
            chunk[chunk.Length - 1] = 1;

            return chunk;
        }

        /// <summary>
        /// Finds right root from array of numbers
        /// And decrypts it(removes the byte header).
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        static byte[] GetDecryptedChunk(BigInteger[] numbers)
        {
            var chunk = numbers.Select(x => x.ToByteArray()).FirstOrDefault(IsChunkValid);

            if (chunk == null)
                throw new InvalidDataException("File coudn't be decrypted");

            var actualDataSize = chunk.Length - bytesHeaderAmount;

            var result = new byte[actualDataSize];

            Array.Copy(chunk, bytesHeaderAmount - 1, result, 0, actualDataSize);

            return result;
        }

        /// <summary>
        /// Check if chunk of data is 
        /// the data that we encrypted
        /// </summary>
        /// <param name="chunk"></param>
        /// <returns></returns>
        static bool IsChunkValid(byte[] chunk)
        {
            return chunk.Last() == 1 && chunk.Take(bytesHeaderAmount - 1).All(x => x == 0);
        }

        #endregion


    }
}
