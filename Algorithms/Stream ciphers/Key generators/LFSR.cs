using System;
using System.Security.Cryptography;
using System.Text;
using CryptoCore.Data;

namespace CryptoCore.Algorithms
{
    /// <summary>
    /// Generator of pseudo-random keys.(Based on LFSR logic) 
    /// <see cref="https://en.wikipedia.org/wiki/Linear-feedback_shift_register"/>
    /// </summary>
    public class LFSR : IKeyGenerator
    {

        #region Private fields

        /// <summary>
        /// Taps used in LFSR
        /// </summary> 
        private int[] Taps { get; set; } = { 27, 8, 7, 1 };

        /// <summary>
        /// Initial state of register
        /// </summary>
        private int InitialState { get; set; }

        /// <summary>
        /// Length of text to cipher in bytes; Length of GeneratedKey = MessageLength 
        /// </summary>
        private long MessageLength { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Initializator
        /// </summary>
        /// <param name="InitialState">Initial state of register</param>
        /// <param name="MessageLength">Length of key to return</param>
        /// <returns>if result was successfull - true, otherwise - false</returns>
        public bool Initialize(string InitialState, long MessageLength)
        {
            var fixedBinaryString = GetFixedBinaryString(InitialState);

            if (fixedBinaryString.Length != Taps[0])
                return false;

            this.InitialState = Convert.ToInt32(fixedBinaryString, 2);

            this.MessageLength = MessageLength;

            return true;
        }

        public bool Initialize(int initialState, long messageLength)
        {
            InitialState = initialState;
            MessageLength = messageLength;

            return true;
        }


        /// <summary>
        /// Initializator
        /// </summary>
        /// <param name="InitialState">Initial state of register</param>
        /// <param name="MessageLength">Length of key to return</param>
        /// <returns>Initial state</returns>
        public bool Initialize(int[] Taps, string InitialState, long MessageLength)
        {

            var fixedBinaryString = GetFixedBinaryString(InitialState);

            if (fixedBinaryString.Length != Taps[0])
                return false;

            this.InitialState = Convert.ToInt32(fixedBinaryString, 2);

            this.MessageLength = MessageLength;

            this.Taps = Taps;

            return true;
        }

        public byte[] GenerateKey()
        {

            var Output = new byte[MessageLength];

            var CurrentRegisterState = InitialState;

            var keyBit = 0;

            // Byte cycle
            for (int i = 0; i < MessageLength; ++i)
            {

                //Bit cycle
                for (int j = 0; j < 8; ++j)
                {

                    int XorResult = 0;

                    //Cycle to get xor'ed result

                    for (int k = 0; k < Taps.Length; ++k)
                    {
                        //shift required bit to lsb, xor w/ result
                        XorResult ^= (CurrentRegisterState >> (Taps[k] - 1));
                    }

                    XorResult &= 1;
                    // Shift to the MSB side
                    CurrentRegisterState = (CurrentRegisterState << 1) | (XorResult);

                    // get shifted byte here
                    keyBit = (CurrentRegisterState >> Taps[0]);

                    //Get MSB by shifting it to LSB
                    Output[i] = (byte)(Output[i] | (keyBit << (8 - j - 1)));

                    //Remove shifted byte

                    CurrentRegisterState &= ~(1 << Taps[0]);
                }

            }

            return Output;
        }

        #endregion


        #region Helper methods
        /// <summary>
        /// Removes all symbols from string except '0' and '1'
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetFixedBinaryString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            var builder = new StringBuilder();

            foreach (var ch in value)
            {
                if ((ch == '0') || (ch == '1'))
                {
                    builder.Append(ch);
                }
            }

            return builder.ToString();
        }

        #endregion
    }
}
