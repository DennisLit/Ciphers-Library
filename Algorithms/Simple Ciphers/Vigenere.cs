using CiphersLibrary.Core;
using System.Linq;

namespace CiphersLibrary.Algorithms
{
    public class Vigenere : ISimpleCipheringAlgorithm
    {

        #region Fields

        public string Alphabet;
        public string keyWord;

        #endregion

        #region Public Methods

        public string Decipher(string stringToDecrypt)
        {
            string Output = string.Empty;
            int j = 0;
            int DecryptedLetterIndx = 0;

            if (string.IsNullOrWhiteSpace(keyWord) && string.IsNullOrWhiteSpace(stringToDecrypt))
                return null;

                for (int i = 0; i < stringToDecrypt.Length; ++i)
                {
                    if (!Alphabet.Contains(stringToDecrypt[i]))
                    {
                        Output += " ";
                        continue;
                    }

                    //Definition of algo: (k + n) mod Alphabet.Length

                    DecryptedLetterIndx = (Alphabet.IndexOf(stringToDecrypt[i]) - Alphabet.IndexOf(keyWord[j]) + Alphabet.Length) % (Alphabet.Length);
                    Output += Alphabet[DecryptedLetterIndx];

                    //Used to cycle through the keyWord

                    ++j;
                    if (j > keyWord.Length - 1) j = 0;

                }
                return Output;
        }

        public string Encrypt(string stringToEncrypt)
        {
            string Output = string.Empty;

            if (string.IsNullOrWhiteSpace(keyWord) && string.IsNullOrWhiteSpace(stringToEncrypt))
                return null;

                int j = 0;
                int EncryptedLetterIndx = 0;

                for (int i = 0; i < stringToEncrypt.Length; ++i)
                {

                    if (!Alphabet.Contains(stringToEncrypt[i]))
                    {
                        Output += " ";
                        continue;
                    }
                        
                        //Definition of algo: (k + n)mod Alphabet.Length

                        EncryptedLetterIndx = (Alphabet.IndexOf(stringToEncrypt[i]) + Alphabet.IndexOf(keyWord[j])) % (Alphabet.Length);
                        Output += Alphabet[EncryptedLetterIndx];

                        //Used to cycle through the keyWord

                        ++j;
                        if (j > keyWord.Length - 1) j = 0;

                }

                return Output;

        }

        public string Initialize(string KeyValue, string Alphabet)
        {
            if (string.IsNullOrWhiteSpace(KeyValue))
                return "Wrong Key!";

            KeyValue = KeyValue.ToUpper();

            for (int i = 0; i < KeyValue.Length; ++i)
            {
                if (!Alphabet.Contains(KeyValue[i]))
                    return "Key value doesnt match the alphabet!";
            }

            this.keyWord = KeyValue;
            this.Alphabet = Alphabet;
            return null;
        }


        #endregion

    }
}
