using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamCipher.Core
{
    public static class ByteExtensions
    {
        /// <summary>
        /// Converts specified array of byte to binary array(WARNING! Works REALLY slow for large arrays)
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static byte[] ConvertToBinaryArray(this byte[] array)
        {
            var builder = new StringBuilder();

            var tmpList = new List<byte>();

            foreach(var x in array)
            {
                builder.Append(Convert.ToString(x,2));
            }
           
            for(int i = 0; i < builder.Length; ++i )
            {
                tmpList.Add((builder[i] == '1') ? (byte)1 : (byte)0);
            }

            return tmpList.ToArray();
        }
    }
}
