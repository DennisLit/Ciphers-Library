using CryptoCore.Core;
using System;
using System.IO;
using System.Text;

namespace CryptoCore.Algorithms
{
    public class RsaSignature : ISignatureCheckingAlgorithm<int>
    {
        #region Ctor

        protected RsaSignature()
        {

        }

        public RsaSignature(int p, int q, int privateKey)
        {
            if ((!p.CheckIfPrime()) || (!q.CheckIfPrime()))
                throw new ArgumentException("p and q must be prime numbers!");

            Modulus = p * q;

            PublicExp = NumericAlgorithms.ExtendedGCD((p - 1) * (q - 1), privateKey);

            PrivateExp = privateKey;
        }

        #endregion

        #region Private props
        private int Modulus { get; set; } = 11 * 19;

        private int PrivateExp { get; set; } = 67;

        public int PublicExp { get; set; } = 43;

        #endregion

        /// <summary>
        /// Method for signing the file with RSA algorithm
        /// </summary>
        /// <param name="compressedMessage"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public virtual int Sign(string fileName)
        {
            var messageToCompress = File.ReadAllBytes(fileName);

            var compressedMessage = HashFunc(messageToCompress);

            var result = HashImage(compressedMessage, PrivateExp);

            using (var fs = new FileStream(GetOutputSignedPath(fileName), FileMode.Create))
            using (var sw = new StreamWriter(fs))
            {
                sw.Write(result.ToString());
            }

            return result;
        }

        public virtual bool CheckSignature(string fileName)
        {
            try
            {
                var signature = File.ReadAllText(GetOutputSignedPath(fileName));
                var textToAnalyze = File.ReadAllText(fileName);

                if (string.IsNullOrWhiteSpace(signature))
                    throw new ArgumentException("Wrong file to analyze!");

                if(!int.TryParse(signature, out var signatureInt))
                {
                    throw new ArgumentException("Wrong signature.");
                }

                return HashFunc(Encoding.Default.GetBytes(textToAnalyze)) == HashImage(signatureInt, PublicExp);
            }
            catch (IOException) { throw; }
            catch (ArgumentException) { throw; }

        }

        /// <summary>
        /// Hash function for signing the file
        /// </summary>
        /// <param name="message"></param>
        /// <param name="startHash"></param>
        /// <returns></returns>
        protected virtual int HashFunc(byte[] message)
        {
            return new YarmolikHash(Modulus).GenerateHash(message);
        }

        /// <summary>
        /// Hash "image" used in RSA digital signature algorithm
        /// </summary>
        /// <param name="message"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        protected virtual int HashImage(int message, int key)
        {
            return NumericAlgorithms.FastExp(message, key, Modulus);
        }

        #region Helper methods

        internal static string GetOutputSignedPath(string InputPath)
        {
            return InputPath.Insert(InputPath.LastIndexOf('.'), "Signed");
        }

        #endregion


    }
}
