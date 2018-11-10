using CryptoCore.Data;
namespace CryptoCore.Core
{
    interface ISimpleCipheringAlgorithmFactory
    {
        ISimpleCipheringAlgorithm NewAlgorithm(SimpleCiphers Cipher);
    }
}
