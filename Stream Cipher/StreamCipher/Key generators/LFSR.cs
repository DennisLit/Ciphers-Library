using System;
using System.Collections;

namespace StreamCipher.Core
{
    /// <summary>
    /// Generator of pseudo-random keys.(Based on LFSR logic)
    /// </summary>
    public class LFSR
    {

        #region Private fields

        /// <summary>
        /// Taps used in LFSR
        /// </summary> 
        private int[] Taps { get; set; } = { 4, 1 };

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
         
        public bool Initialize(string InitialState, long MessageLength)
        {
            if (InitialState.Length != Taps[0])
                return false;

            this.InitialState = Convert.ToInt32(InitialState,2);

            this.MessageLength = MessageLength;

            return true;
        }

        public bool Initialize(int[] Taps, string InitialState, long MessageLength)
        {
            if (InitialState.Length != Taps[0])
                return false;

            this.InitialState = Convert.ToInt32(InitialState, 2);

            this.MessageLength = MessageLength;

            this.Taps = Taps;

            return true;
        }

        public byte[] GenerateKey()
        {

            var Output = new byte[MessageLength];

            var CurrentRegisterState = InitialState;
            // Byte cycle
            for (int i = 0; i < MessageLength; ++i)
            {
                //Bit cycle
                for(int j = 0; j < 8; ++j)
                {
                    Output[i] = (byte)( Output[i] | ((CurrentRegisterState & 1) << ( 8 - j - 1)));

                    int XorResult = 0;

                    //Cycle to get xor'ed result

                    for (int k = 0; k < Taps.Length; ++k)
                    {
                        //shift required bit to lsb, xor w/ result
                        XorResult ^= (CurrentRegisterState >> (Taps[k] - 1));
                    }

                    XorResult &= 1;

                    // Shift to the LSB side
                    CurrentRegisterState = (XorResult << Taps[0] - 1) | (CurrentRegisterState >> 1);
                }

            }

            return Output;
        }

        #endregion

    }
}
