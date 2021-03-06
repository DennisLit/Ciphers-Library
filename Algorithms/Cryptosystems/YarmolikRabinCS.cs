﻿using CiphersLibrary.Core;
using System;
using System.IO;
using System.Text;

namespace CiphersLibrary.Algorithms
{
    /// <summary>
    /// Yarmolik - Rabin cryptosystem
    /// </summary>
    public class YarmolikRabinCS : IYarmolikRabinCryptoSystem
    {
        private readonly int maxRusUnicodeIndx = 1100;

        #region Public methods

        /// <summary>
        /// Encrypts a file specified in filepath parameter
        /// </summary>
        public void Encrypt(int p, int q, int b, string filePath)
        {
            Encrypt(CalculatePublicKey(p, q), b, filePath);
        }
        /// <summary>
        /// Encrypts a file specified in filepath parameter
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="filePath"></param>
        public void Encrypt(int publicKey, int b, string filePath)
        {
            try
            {
                string ToEncrypt = File.ReadAllText(filePath, Encoding.Default);

                using (var FirstFileStream = new FileStream(GetOutputEncryptedPath(filePath), FileMode.Create, FileAccess.Write))
                using (var SecondFileStream = new FileStream(GetOutputEncryptedSymbolsPath(filePath), FileMode.Create, FileAccess.Write))
                using (var SWriter = new StreamWriter(FirstFileStream))
                using (var secWriter = new StreamWriter(SecondFileStream))
                {

                    foreach (var ch in ToEncrypt)
                    {
                        var result = Encrypt(publicKey, b, Convert.ToInt32(ch));
                        SWriter.Write(result.ToString() + " ");
                        var output = Encoding.Unicode.GetString(BitConverter.GetBytes(result));
                        secWriter.Write(output);
                    }

                }
            }
            catch (ArgumentException ex)
            { throw ex; }

        }
        /// <summary>
        /// Decrypts a message and writes it to filePath location with modified name
        /// </summary>
        /// <param name="p">Secret key</param>
        /// <param name="q">Secret key</param>
        /// <param name="filePath">Path to write the file(including file name)</param>
        public void Decrypt(int p, int q, int b, string filePath)
        {
            try
            {
                using (var FsStream = new FileStream(filePath, FileMode.Open))
                using (var FirstFileStream = new FileStream(GetOutputDecryptedPath(filePath), FileMode.Create, FileAccess.Write))
                using (var SecondFileStream = new FileStream(GetOutputDecryptedSymbolsPath(filePath), FileMode.Create, FileAccess.Write))
                using (var bWriter = new StreamWriter(FirstFileStream))
                using (var secWriter = new StreamWriter(SecondFileStream))
                using (var sr = new StreamReader(FsStream))
                {
                    string cipherText;

                    while ((cipherText = GetEncryptedChunk(sr)) != null)
                    {
                        int.TryParse(cipherText, out int parseResult);
                        var results = Decrypt(p, q, b, parseResult);

                        foreach (var result in results)
                        {
                            bWriter.Write(Encoding.Unicode.GetString(BitConverter.GetBytes(result)));
                            secWriter.Write(result.ToString() + " ");
                        }

                        bWriter.Write(Environment.NewLine);
                        secWriter.Write(Environment.NewLine);
                    }

                }
            }
            catch (ArgumentException)
            {
                throw;
            }
        }

        #endregion

        #region Encryption / Decryption main methods
        /// <summary>
        /// Encrypts a message
        /// </summary>
        /// <param name="FirstPrime"> First prime (part of secret key)</param>
        /// <param name="SecondPrime">Second prime (part of secret key)</param>
        /// <param name="Message">Message to encrypt</param>
        /// <returns></returns>
        private long Encrypt(int publicKey, int b, int plainText)
        {
            return (plainText < publicKey) ? (plainText * (plainText + b)) % publicKey : throw new ArgumentException($"Length of public key must be more than {maxRusUnicodeIndx}!");
        }

        private int[] Decrypt(int FirstPrime, int SecondPrime, int b, int cipherText)
        {
            var publicKey = CalculatePublicKey(FirstPrime, SecondPrime);

            if (cipherText > publicKey)
                throw new ArgumentException("cipherText", "Ciphertext is larger then public key");

            var D = (b * b + (4 * cipherText)) % publicKey;

            int mp = NumericAlgorithms.FastExp(D, (FirstPrime + 1) / 4, FirstPrime);
            int mq = NumericAlgorithms.FastExp(D, (SecondPrime + 1) / 4, SecondPrime);


            NumericAlgorithms.ExtendedGCD(FirstPrime, SecondPrime, out int yp, out int yq);

            int d1, d2, d3, d4;

            d1 = (yp * FirstPrime * mq + yq * SecondPrime * mp) % publicKey;
            d1 = (d1 >= 0) ? d1 : NumericAlgorithms.NegativeMod(yp * FirstPrime * mq + yq * SecondPrime * mp, publicKey);
            // do { d1 += publicKey; } while (d1 <= 0);

            d2 = publicKey - d1;

            d3 = (yp * FirstPrime * mq - yq * SecondPrime * mp) % publicKey;
            d3 = (d3 >= 0) ? d3 : NumericAlgorithms.NegativeMod(yp * FirstPrime * mq - yq * SecondPrime * mp, publicKey);
            //do { d3 += publicKey; } while (d3 <= 0);

            d4 = publicKey - d3;

            d1 = Math.Abs(CalculateRoot(b, d1, publicKey));
            d2 = Math.Abs(CalculateRoot(b, d2, publicKey));
            d3 = Math.Abs(CalculateRoot(b, d3, publicKey));
            d4 = Math.Abs(CalculateRoot(b, d4, publicKey));

            return new int[] { d1, d2, d3, d4 };

        }


        #endregion

        #region Helper path methods

        private static string GetOutputEncryptedPath(string InputPath)
        {
            return InputPath.Insert(InputPath.LastIndexOf('.'), "Encrypted");
        }

        private static string GetOutputEncryptedSymbolsPath(string InputPath)
        {
            return InputPath.Insert(InputPath.LastIndexOf('.'), "EncryptedSymbols");
        }

        private static string GetOutputDecryptedPath(string InputPath)
        {
            return InputPath.Insert(InputPath.LastIndexOf('.'), "Decrypted");
        }

        private static string GetOutputDecryptedSymbolsPath(string InputPath)
        {
            return InputPath.Insert(InputPath.LastIndexOf('.'), "DecryptedSymbols");
        }


        #endregion

        #region Misc

        private int CalculatePublicKey(int firstPrime, int secondPrime)
        {
            return firstPrime * secondPrime;
        }

        public static string GetEncryptedChunk(StreamReader reader)
        {
            string chunk = string.Empty;
            int currSymbol = 0;

            while ((currSymbol = reader.Read()) != ' ')
            {
                //if reader have reached the end of stream it returns -1.

                if (currSymbol == -1)
                    return null;

                chunk += (char)currSymbol;
            }

            return chunk;
        }

        public static int CalculateRoot(int b, int lastRoot, int publicKey)
        {
            if (((lastRoot - b) % 2) == 0)
                return ((-b + lastRoot) / 2) % publicKey;
            else
                return ((-b + publicKey + lastRoot) / 2) % publicKey;
        }

        #endregion

    }
}
