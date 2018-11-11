using CiphersLibrary.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CiphersLibrary.Algorithms
{
    public class ColumnarTransposition : ISimpleCipheringAlgorithm
    {
        #region Helper Data

        /// <summary>
        /// Struct used as a helper data for re-arranging the columns
        /// </summary>
        public struct ColumnsPlacement
        {
            public int CurrentPosition;
            public int RightPosition;
        }

        #endregion

        #region Private fields

        /// <summary>
        /// Key word for rearranging the columns
        /// </summary>
        private string keyWord;

        /// <summary>
        /// Word provided by user
        /// </summary>
        private string Word;

        /// <summary>
        /// Rows needed in matrix
        /// </summary>
        private int RowsAmnt;
        private string Alphabet;
        private List<ColumnsPlacement> colsOrder;
        private List<List<char>> ColumnsArr;

        #endregion

        #region Public methods

        public string Decipher(string stringToDecrypt)
        {
            Word = string.IsNullOrWhiteSpace(stringToDecrypt) ? Word : stringToDecrypt;
            RowsAmnt = ((Word.Length + keyWord.Length - 1) / keyWord.Length); // formula for calculating the rows
            InitializeMatrix();
            return CreateDecipherOutput();
        }

        public string Encrypt(string stringToEncrypt)
        {
            Word = string.IsNullOrWhiteSpace(stringToEncrypt) ? Word : stringToEncrypt;
            RowsAmnt = ((Word.Length + keyWord.Length - 1) / keyWord.Length); // formula for calculating the rows
            NewEncryptMatrix();
            return CreateEncryptOutput();
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

            for (int i = 0; i < KeyValue.Length; ++i)
            {
                if (KeyValue.IndexOf(KeyValue[i]) != KeyValue.LastIndexOf(KeyValue[i]))
                    return "Key value must contain unique letters!";
            }

            this.keyWord = KeyValue;
            this.Alphabet = Alphabet;

            return null;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Method used by Decipher method, to allocate needed memory
        /// </summary>
        private void InitializeMatrix()
        {
            ColumnsArr = new List<List<char>>();

            for (int i = 0; i < RowsAmnt; ++i)
            {
                ColumnsArr.Add(new List<char>());

                for (int j = 0; j < keyWord.Length; ++j)
                {
                    ColumnsArr[i].Add('\0');
                }
            }
        }

        /// <summary>
        /// Method used by Encrypt method, 
        /// it does two things: allocates memory, fills the matrix with the word
        /// </summary>
        private void NewEncryptMatrix()
        {
            int WordIndx = 0;

            ColumnsArr = new List<List<char>>();

            for (int i = 0; i < RowsAmnt; ++i)
            {
                ColumnsArr.Add(new List<char>());

                for (int j = 0; j < keyWord.Length; ++j)
                {
                    // fill random letters in gap, if it's present

                    if (WordIndx < Word.Length)
                    {
                        ColumnsArr[i].Add(Word[WordIndx]);
                        ++WordIndx; 
                    }
                    else
                    {
                        // completing the matrix with '/'
                        ColumnsArr[i].Add(' ');
                    }
                }
            }
        }


        private string CreateEncryptOutput()
        {
            InitColumnPlacement();

            int RememberedI = 0;

            int CurrentColumn = 0;

            var Output = string.Empty;

            for (int i = 0; i < keyWord.Length; ++i)
            {
                //remember i to know if  i < keyWord.Length 
                //is still OK later
                RememberedI = i;

                //set i as column to output(using 1 variable there, should use 2 for readability)
                i = colsOrder[CurrentColumn].CurrentPosition;

                for (int j = 0; j < RowsAmnt; ++j)
                {
                    Output += ColumnsArr[j][i];
                }

                //update i from remembered I
                i = RememberedI;

                ++CurrentColumn;
            }

            return Output;
        }

        /// <summary>
        /// Creates deciphered string from a word variable.
        /// </summary>
        /// <returns></returns>
        private string CreateDecipherOutput()
        {
            InitColumnPlacement();

            colsOrder.Sort((x, y) => x.CurrentPosition.CompareTo(y.CurrentPosition));

            int CurrentColumn = 0;

            int WordIndx = 0;

            //Decipher the ciphered word by re-arranging the columns

            for (int i = 0; i < keyWord.Length; ++i)
            {
                //our ciphered word consists of some number of blocks(rows)

                WordIndx = colsOrder[CurrentColumn].RightPosition * RowsAmnt;

                for (int j = 0; (j < RowsAmnt) && (WordIndx < Word.Length); ++j, ++WordIndx)
                {
                    ColumnsArr[j][i] = Word[WordIndx];
                }

                ++CurrentColumn;
            }

            var Output = string.Empty;

            //Output deciphered word.

            for (int i = 0; i < RowsAmnt; ++i)
            {
                for (int j = 0; j < keyWord.Length; ++j)
                {
                    if (ColumnsArr[i][j] == '/')
                        return Output;
                    
                    Output += ColumnsArr[i][j];
                }
            }

            return Output;

        }

        /// <summary>
        /// This helper method is for encrypt method to output the encrypted array.
        /// Sets accordance -- Current column index --> Index of a column to output
        /// in colsOrder array 
        /// <returns> Returns index of start of the colsOrder array </returns>
        /// </summary>
        private void InitColumnPlacement()
        {
            colsOrder = new List<ColumnsPlacement>();

            char[] characters = keyWord.ToArray();

            Array.Sort(characters);

            // alphabetically right string

            string tmpStr = new string(characters);

            for (int i = 0; i < keyWord.Length; ++i)
            {                                   // example:: ZEBRAS -- A has index 4 so we output 4-th column 
                                                // first because A has 1 st Alphabet position in keyword
                colsOrder.Add(new ColumnsPlacement() { RightPosition = tmpStr.IndexOf(keyWord[i]), CurrentPosition = i });
            }

            colsOrder.Sort((x, y) => x.RightPosition.CompareTo(y.RightPosition));

        }


        #endregion
    }
}
