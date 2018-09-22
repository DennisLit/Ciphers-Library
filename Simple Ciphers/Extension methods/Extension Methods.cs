using System;

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

        public static int ReplaceFirstBit(this int value, int bit, int PolynomLength)
        {
            string baseString = Convert.ToString(value, 2);

            baseString = baseString.PadLeft(PolynomLength, '0');

            baseString = bit.ToString() + baseString.Substring(1);

            return value = Convert.ToInt32(baseString, 2);
        }
    }
}
