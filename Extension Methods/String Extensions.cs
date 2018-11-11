using System;

namespace CiphersLibrary.Core
{ 
    public static class ExtensionMethods
    {
        public static string Truncate(this string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }
    }
}
