using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiphersLibrary.Algorithms
{
    public class Sha256Signature : RsaSignature
    {

        public Sha256Signature() : base()
        {
            
        }

        /// <summary>
        /// Method accepts file name of analyzed file, 
        /// his signature should exist by the same name with "signed" at the end.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public override bool CheckSignature(string fileName)
        {
            try
            {
                var signature = File.ReadAllText(GetOutputSignedPath(fileName));
                var textToAnalyze = File.ReadAllText(fileName);

                if (string.IsNullOrWhiteSpace(signature))
                    throw new ArgumentException("Wrong file to analyze!");

                var hashResult = HashFunc(Encoding.UTF8.GetBytes(textToAnalyze));
                var hashString = string.Empty;

                for (int i = 0; i < hashResult.Length; ++i)
                {
                    hashString += hashResult[i].ToString("X");
                }

                return hashString.Equals(signature);
            }
            catch (IOException) { throw; }
            catch (ArgumentException) { throw; }

        }

        /// <summary>
        /// Used to create file signature. 
        /// Creates new file with same name as provided, but with "signed" at the end.
        /// </summary>
        /// <param name="fileName"></param>
        public new void Sign(string fileName)
        {
            var hashArray = HashFunc(File.ReadAllBytes(fileName));

            using (var fs = new FileStream(GetOutputSignedPath(fileName), FileMode.Create))
            using (var sw = new StreamWriter(fs))
            {               
                for (int i = 0; i < hashArray.Length; ++i)
                {
                    sw.Write(hashArray[i].ToString("X"));
                }
            }
        }

        protected new uint[] HashFunc(byte[] message)
        {
            var hashFunc = new Sha256();

            var result = hashFunc.GenerateHash(message);

            return result;
        }
    }
}
