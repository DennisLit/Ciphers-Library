using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCore.Algorithms
{
    public class Geffe : IKeyGenerator
    {
        public Geffe()
        {
            FirstLfsr = new LFSR();
            SecondLfsr = new LFSR();
            ThirdLfsr = new LFSR();
        }

        #region Default taps

        private readonly int[] Taps1 = { 28, 3, 1};
        private readonly int[] Taps2 = { 24, 4, 3, 1 };
        private readonly int[] Taps3 = { 27, 8, 7, 1 };

        #endregion

        #region Initial states of LFSRs used

        private int InitialState1 { get; set; }

        private int InitialState2 { get; set; }

        private int InitialState3 { get; set; }

        #endregion

        #region Private fields
        private long BytesMessageLength { get; set; }
        #endregion

        #region LFSR instances

        private LFSR FirstLfsr { get; set; }

        private LFSR SecondLfsr { get; set; }

        private LFSR ThirdLfsr { get; set; }

        #endregion

        public bool Initialize(string InitialState, long bytesMessageLength)
        {
            var fixedBinaryString = GetFixedBinaryString(InitialState);

            if (fixedBinaryString.Length != Taps2[0])
                return false;

            InitialState2 = Convert.ToInt32(InitialState, 2);
            InitialState1 = Convert.ToInt32(InitialState.PadLeft(Taps1[0], '1'), 2);
            InitialState3 = Convert.ToInt32(InitialState.PadLeft(Taps3[0], '1'), 2);

            BytesMessageLength = bytesMessageLength;

            FirstLfsr.Initialize(InitialState1, BytesMessageLength);
            SecondLfsr.Initialize(InitialState2, BytesMessageLength);
            ThirdLfsr.Initialize(InitialState2, BytesMessageLength);

            return true;

        }

        public byte[] GenerateKey()
        {
            byte[] firstKey, secondKey, thirdKey;

            firstKey = FirstLfsr.GenerateKey();
            secondKey = SecondLfsr.GenerateKey();       
            thirdKey = ThirdLfsr.GenerateKey();

            var resultKey = new byte[BytesMessageLength];

            for(int i = 0; i < BytesMessageLength; ++i)
            {
                resultKey[i] = (byte)((firstKey[i] & secondKey[i]) | (~firstKey[i] & thirdKey[i]));
            }

            return resultKey;
        } 

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
