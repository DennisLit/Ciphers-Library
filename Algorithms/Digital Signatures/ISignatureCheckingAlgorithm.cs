namespace CiphersLibrary.Algorithms
{
    interface ISignatureCheckingAlgorithm<T>
    {
        T Sign(string filePath);
        bool CheckSignature(string filePath);
    }
}
