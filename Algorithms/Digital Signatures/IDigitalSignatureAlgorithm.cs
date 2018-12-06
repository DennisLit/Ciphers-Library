namespace CiphersLibrary.Algorithms
{
    /// <summary>
    /// Algorithm that is used to leave the signature and check the signature of a file
    /// </summary>
    /// <typeparam name="T">Type of number used to sign the file(i.e BigInteger)</typeparam>
    interface IDigitalSignatureAlgorithm<T>
    {
        T Sign(string filePath);
        bool CheckSignature(string filePath);
    }
}
