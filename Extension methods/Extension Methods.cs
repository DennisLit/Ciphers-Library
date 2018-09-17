using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCiphers
{
    public static class ExtensionMethods
    {
        public static string Truncate(this string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }

        public static int IndexOfLast(this string value, char charToFind)
        {
            for(int i = value.Length - 1; i > 0; --i)
            {
                if (value[i] == charToFind)
                    return i;
            }

            return -1;
        }
    }
}
