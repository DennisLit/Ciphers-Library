using CryptoCore.Core;
using CryptoCore.Helpers;
using System;

namespace CryptoCore.Algorithms
{

    /// <summary>
    /// This algorithm is unfinished. 
    /// Works only with texts up to 16 symbols of length.
    /// </summary>
    public class TurningGrille : ISimpleCipheringAlgorithm
    {
        /// <summary>
        /// Grille cells to cut out
        /// </summary>
        #region Helper data
        public struct PlacePattern
        {
            public int Row;
            public int Column;
        }
        #endregion

        #region Fields

        public string Alphabet;
        public string Word;
        public PlacePattern[] Pattern = new PlacePattern[MatrixColsAmnt];

        private const int MatrixRowsAmnt = 4;
        private const int MatrixColsAmnt = 4;
        private const int matrixRowsAmnt = MatrixRowsAmnt;
        private char[,] MainMatrix = new char[MatrixRowsAmnt, MatrixColsAmnt];

        #endregion

        #region Public methods

        public string Decipher(string stringToDecrypt)
        {
            Word = stringToDecrypt;
            return CreateDecryptedText();
        }

        public string Encrypt(string stringToEncrypt)
        {
            Word = stringToEncrypt;
            return CreateEncryptedText();
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Default pattern for 4x4 matrix
        /// </summary>
        private void InitPattern()
        {
            Pattern[0].Column = 0;
            Pattern[0].Row = 0;

            Pattern[1].Column = 3;
            Pattern[1].Row = 1;

            Pattern[2].Column = 2;
            Pattern[2].Row = 2;

            Pattern[3].Column = 1;
            Pattern[3].Row = 3;
        }

        private void ClearMainMatrix()
        {
            for (int i = 0; i < MatrixColsAmnt; ++i)
            {
                for (int j = 0; j < MatrixRowsAmnt; ++j)
                {
                    MainMatrix[i, j] = '#';
                }
            }
        }

        private string CreateEncryptedText()
        {
            var Output = string.Empty;

            int SymbolsWroteCount = 0;
            Random randomSymbol = new Random();
            //MatrixColsAmnt == number of rotates 
            for (int i = 0; i < MatrixColsAmnt; ++i)
            {
                for (int j = 0; j < MatrixRowsAmnt; ++j, ++SymbolsWroteCount)
                {
                    MainMatrix[Pattern[j].Row, Pattern[j].Column] =
                    (SymbolsWroteCount >= Word.Length) ?
                    //if we wrote the whole word already, start filling in random literals
                    (char)randomSymbol.Next(Alphabet[0], Alphabet[Alphabet.Length - 1]) : Word[SymbolsWroteCount];
                }

                MainMatrix = MatrixHelper.RotateRight(ref MainMatrix, MatrixRowsAmnt, MatrixColsAmnt);
            }

            // if word's length is not 16
            // fill in random letters in matrix

            for (int i = 0; i < MatrixColsAmnt; ++i)
            {
                for (int j = 0; j < MatrixRowsAmnt; ++j)
                {
                    Output += MainMatrix[i, j];
                }
            }

            //Clear the matrix for the next use

            ClearMainMatrix();

            return Output;
        }

        private string CreateDecryptedText()
        {
            int SymbolIndx = 0;
            var Output = string.Empty;

            //Write word to a grille

            for (int i = 0; i < MatrixColsAmnt; ++i)
            {
                for (int j = 0; j < MatrixRowsAmnt; ++j, ++SymbolIndx)
                {
                    MainMatrix[i, j] = Word[SymbolIndx];
                }
            }

            //decrypt the word
            //MatrixColsAmnt == number of rotates

            for (int i = 0; i < MatrixColsAmnt; ++i)
            {
                for (int j = 0; j < MatrixRowsAmnt; ++j)
                {
                    Output += MainMatrix[Pattern[j].Row, Pattern[j].Column];
                }

                MainMatrix = MatrixHelper.RotateRight(ref MainMatrix, MatrixRowsAmnt, MatrixColsAmnt);
            }

            return Output;
        }

        public string Initialize(string KeyValue, string Alphabet)
        {
            if (string.IsNullOrWhiteSpace(Alphabet))
                return "No alphabet was provided.";

            InitPattern();

            ClearMainMatrix();

            this.Alphabet = Alphabet;

            return null;
        }

        #endregion
    }
}
