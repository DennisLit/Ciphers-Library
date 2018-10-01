using System.Collections.Generic;

namespace StreamCipher.Core
{
    /// <summary>
    /// Stream cipher class.
    /// </summary>
    public static class MainStreamCipher 
    { 
        /// <summary>
        /// Main xor function of stream cipher
        /// </summary>
        /// <param name="Text">byte array of plaintext</param>
        /// <param name="key">byte array of key</param>
        /// <returns></returns>
        public static byte[] EncryptDecrypt(byte[] Text, byte[] key)
        {
            var ListToReturn = new List<byte>();

            for(int i = 0; i < Text.Length; ++i)
            {
                ListToReturn.Add((byte)(Text[i] ^ key[i]));
            }

            return ListToReturn.ToArray();
        }
    }
}
