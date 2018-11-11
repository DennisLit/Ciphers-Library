using CiphersLibrary.Data;
namespace CiphersLibrary.Core
{
    interface ISimpleCipheringAlgorithmFactory
    {
        ISimpleCipheringAlgorithm NewAlgorithm(SimpleCiphers Cipher);
    }
}
