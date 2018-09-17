namespace SimpleCiphers
{
    public interface ICipheringAlgorithm
    {
        /// <summary>
        /// Encrypt function
        /// </summary>
        /// <param name="stringToEncrypt"></param>
        /// <returns></returns>
        string Encrypt(string stringToEncrypt);

        /// <summary>
        /// Decipher function
        /// </summary>
        /// <param name="stringToDecrypt"></param>
        /// <returns></returns>
        string Decipher(string stringToDecrypt);

        /// <summary>
        /// Used to provide data for algorithm
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="Alphabet"></param>
        /// <param name="Word"></param>
        /// <returns>Returns error message, if it's null, then initialized successfully</returns>
        string Initialize(string KeyValue, string Alphabet);

    }
}
