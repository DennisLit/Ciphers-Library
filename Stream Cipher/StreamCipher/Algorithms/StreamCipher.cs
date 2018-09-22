using System.Collections.Generic;

namespace StreamCipher.Core
{
    public static class MainStreamCipher 
    { 
        public static List<byte> EncryptDecrypt(List<byte> Text, List<byte> key)
        {
            var ListToReturn = new List<byte>();

            for(int i = 0; i < Text.Count; ++i)
            {
                ListToReturn.Add((byte)(Text[i] ^ key[i]));
            }

            return ListToReturn;
        }
    }
}
