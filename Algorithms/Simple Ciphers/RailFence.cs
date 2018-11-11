using CiphersLibrary.Core;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CiphersLibrary.Algorithms
{
    public class RailFence : ISimpleCipheringAlgorithm
    {

        #region Helper Data
        /// <summary>
        /// Used to output data fast without iterating through whole matrix
        /// </summary>
        public struct Letters
        {
            public int Col;
            public int Row;
            public char Symbol;
        }

        #endregion

        #region Fields

        private string Word;
        private int fenceHeight;
        private int rowLength;
        // fence itself

        private List<List<char>> Fence;

        //letters and their positions for fast output

        private List<Letters> letters = new List<Letters>();

        #endregion

        #region Public methods

        public string Decipher(string stringToDecrypt)
        {
            Word = stringToDecrypt;
            return CreateDecipherOutput();
        }

        public string Encrypt(string stringToEncrypt)
        {
            rowLength = stringToEncrypt.Length;

            ClearArray();

            Word = stringToEncrypt;

            string Output = string.Empty;

            int i = -1;
            int j = -1;
            int WordIndx = 0;
            bool isNextIter = false;
            bool isStartIter = true;
            int LettersIndx = 0;

            //Algorithm here is like bouncing ball -- 
            //Where IsStartIter is moment where we throw a ball, IsNextIter -- 
            //where it bounces off the wall
            for (; WordIndx < Word.Length; ++WordIndx)
            {
                //ZIG
                if (isNextIter)
                {
                    --i;
                    ++j;
                    Fence[i][j] = Word[WordIndx];
                    letters.Add(new Letters() { Col = j, Row = i, Symbol = Word[WordIndx] });
                    ++LettersIndx;
                }
                else
                {
                    //ZAG
                    if (isStartIter)
                    {
                        ++i;
                        ++j;
                        Fence[i][j] = Word[WordIndx];
                        letters.Add(new Letters() { Col = j, Row = i, Symbol = Word[WordIndx] });
                        ++LettersIndx;
                    }
                }
                if (i == fenceHeight - 1)
                { isNextIter = true; isStartIter = false; }

                if (i == 0)
                { isStartIter = true; isNextIter = false; }
            }

            //Output created Matrix

            for (j = 0; j < fenceHeight; ++j)
            {
                for (i = 0; i < Word.Length; ++i)
                {
                    if (letters[i].Row == j)
                        Output += letters[i].Symbol;
                }
            }

            return Output;
        }

        public string Initialize(string KeyValue, string Alphabet)
        {
            if (!int.TryParse(KeyValue, out int fenceHeight))
               return "Wrong fence height value!";

            if ((fenceHeight > 100) || (fenceHeight <= 1))
                return "Wrong fence height value :(!";

            this.fenceHeight = fenceHeight;

            return null;
        }

        #endregion

        #region Helper methods

        private void ClearArray()
        {
            // Get memory for our array and initialize it

            Fence = new List<List<char>>();

            for (int k = 0; k < fenceHeight; ++k)
            {
                Fence.Add(new List<char>());

                for(int j = 0; j < rowLength; ++j)
                {
                    Fence[k].Add(new char());
                }

            }

        }

        private void InitDecipherMatrix()
        {
            rowLength = Word.Length;

            ClearArray();

            int i = -1;
            int j = -1;
            bool isNextIter = false;
            bool isStartIter = true;
            int WordIndx = 0; 

            // writing word from row to row till the end
            for (int RowToFill = 0; RowToFill < fenceHeight; ++RowToFill)
            {
                isNextIter = false;
                isStartIter = true;
                i = -1;
                j = -1;
                for (; (j < Word.Length - 1) && (WordIndx < Word.Length);)
                {
                    //ZIG
                    if (isNextIter)
                    {
                        --i;
                        ++j;
                        if (RowToFill == i)
                        {
                            Fence[i][j] = Word[WordIndx];
                            ++WordIndx;
                        }
                    }
                    else
                    {
                        //ZAG
                        if (isStartIter)
                        {
                            ++i;
                            ++j;
                            if (RowToFill == i)
                            {
                                Fence[i][j] = Word[WordIndx];
                                ++WordIndx;
                            }
                        }
                    }

                    if (i == fenceHeight - 1)
                    { isNextIter = true; isStartIter = false; }

                    if (i == 0)
                    { isStartIter = true; isNextIter = false; }
                }
            }

        }

        private string CreateDecipherOutput()
        {
            //Write word to matrix

            InitDecipherMatrix();
            
            var Output = string.Empty;

            int i = -1;
            int j = -1;
            bool isNextIter = false;
            bool isStartIter = true;
            int WordIndx = 0;

            //read word from matrix

            for (; WordIndx < Word.Length; ++WordIndx)
            {
                if (isNextIter)
                {
                    --i;
                    ++j;
                    Output += Fence[i][j];
                }
                else
                {
                    if (isStartIter)
                    {
                        ++i;
                        ++j;
                        Output += Fence[i][j];
                    }
                }

                if (i == fenceHeight - 1)
                { isNextIter = true; isStartIter = false; }

                if (i == 0)
                { isStartIter = true; isNextIter = false; }
            }

            return Output;
        }

        #endregion
    }
}
