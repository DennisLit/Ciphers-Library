using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamCipher 
{
    /// <summary>
    /// Generator of keys for Stream cipher.(Based on LFSR logic)
    /// </summary>
    public class LFSR
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
        /// Length of text to cipher in bits; Length of GeneratedKey = MessageLength 
        /// </summary>
        private int MessageLength { get; set; }

        #endregion

        public bool Initialize(string InitialState, int MessageLength)
        {
            if (InitialState.Length < Taps[0])
                return false;

            this.InitialState = Convert.ToInt32(InitialState,2);

            this.MessageLength = MessageLength;

            return true;
        }

        public bool Initialize(int[] Taps, string InitialState, int MessageLength)
        {
            if (InitialState.Length < Taps[0])
                return false;

            this.InitialState = Convert.ToInt32(InitialState, 2);

            this.MessageLength = MessageLength;

            this.Taps = Taps;

            return true;
        }

        public BitArray GenerateKey()
        {

            var Output = new BitArray(MessageLength);

            var CurrentRegisterState = InitialState;

            for (int i = 0; i < MessageLength; ++i)
            {
                Output[i] = ((CurrentRegisterState & 1) == 1) ? true: false;
                                                                 //Move result of xor to MSB   // Shift to the LSB side
                CurrentRegisterState = (XorBits(CurrentRegisterState) << Taps[0] - 1) | (CurrentRegisterState >> 1);
            }

            return Output;
        }

        public int XorBits(int State)
        {
            int result = 0;

            for(int i = 0; i < Taps.Length; ++i)
            {
                result ^= (State >> (Taps[i] - 1)); 
            }

            return result & 1;
        }


    }
}
